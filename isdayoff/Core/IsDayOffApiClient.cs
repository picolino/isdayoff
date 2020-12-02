using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Exceptions;
using isdayoff.Core.Extensions;
using isdayoff.Core.Http;
using isdayoff.Core.Responses;
using isdayoff.Core.Tracing;

namespace isdayoff.Core
{
    internal class IsDayOffApiClient : IIsDayOffApiClient
    {
        private readonly HttpClientFactory httpClientFactory;
        private readonly string baseUrl;
        private readonly string userAgent;

        public IsDayOffApiClient(string baseUrl, string userAgent, HttpClientFactory httpClientFactory)
        {
            this.baseUrl = baseUrl;
            this.userAgent = userAgent;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<GetDataApiResponse> GetDataAsync(DateTime from, DateTime to, Country country, CancellationToken cancellationToken)
        {
            return await GetDataInternalAsync(from, to, country, cancellationToken);
        }
        
        private async Task<GetDataApiResponse> GetDataInternalAsync(DateTime from, DateTime to, Country country, CancellationToken cancellationToken)
        {
            using (var httpClient = httpClientFactory.CreateHttpClient())
            {
                var countryCode = GetCountryCode(country);
                
                var requestUrl = BuildGetDataRequestUrl(from, to, countryCode);

                try
                {
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

                    IsDayOff.Tracer.TraceEvent(TraceEventType.Information, TraceEventIds.Requesting.REQUEST_SENDING,
                                               "Sending HTTP GET: '{0}'", requestUrl);
                    
                    var response = await httpClient.GetAsync(requestUrl, cancellationToken);
                    var responseAsString = await response.Content.ReadAsStringAsync();

                    IsDayOff.Tracer.TraceEvent(TraceEventType.Information, TraceEventIds.Requesting.REQUEST_SENT,
                                               "Response received with status code: '{0}' and string content: '{1}'", response.StatusCode, responseAsString);

                    ValidateResponse(from, to, country, response, responseAsString);

                    return new GetDataApiResponse(responseAsString);
                }
                catch (Exception e)
                {
                    IsDayOff.Tracer.TraceEvent(TraceEventType.Error, TraceEventIds.Requesting.REQUEST_SENDING_ERROR,
                                               "An error occured while processing request: '{0}'\n{1}", requestUrl, e);
                    
                    throw new IsDayOffExternalServiceException(e);
                }
            }
        }

        private string BuildGetDataRequestUrl(DateTime from, DateTime to, string countryCode)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(baseUrl);
            stringBuilder.Append("getdata");
            stringBuilder.Append("?date1=");
            stringBuilder.AppendFormat("{0:yyyyMMdd}", from);
            stringBuilder.Append("&date2=");
            stringBuilder.AppendFormat("{0:yyyyMMdd}", to);
            stringBuilder.Append("&cc=");
            stringBuilder.Append(countryCode);

            return stringBuilder.ToString();
        }

        private static void ValidateResponse(DateTime from, DateTime to, Country country, HttpResponseMessage response, string responseContent)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest when responseContent == IsDayOffServiceConstants.BadDateResponseCode:
                    throw new BadDatesRangeException(from, to, country);
                case HttpStatusCode.BadRequest when responseContent == IsDayOffServiceConstants.ServiceErrorResponseCode:
                    throw new ServiceErrorException();
                case HttpStatusCode.NotFound when responseContent == IsDayOffServiceConstants.DayOffDataNotFoundResponseCode:
                    throw new DayOffDataNotFoundException(from, to, country);
                default:
                    response.EnsureSuccessStatusCode();
                    break;
            }
            
            var requestedDaysCount = from.ByDaysTill(to).Count();
            if (requestedDaysCount > responseContent.Length)
            {
                throw new DaysCountMismatchException(requestedDaysCount, responseContent.Length);
            }
        }
        
        private static string GetCountryCode(Country country)
        {
            switch (country)
            {
                case Country.Russia:
                    return "ru";
                case Country.Belarus:
                    return "by";
                case Country.Ukraine:
                    return "ua";
                case Country.Kazakhstan:
                    return "kz";
                case Country.USA:
                    return "us";
                default:
                    throw new ArgumentOutOfRangeException(nameof(country), country, ErrorsMessages.UnknownCountry());
            }
        }
    }
}