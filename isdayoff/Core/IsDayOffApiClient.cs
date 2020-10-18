using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Exceptions;
using isdayoff.Core.Http;
using isdayoff.Core.Responses;

namespace isdayoff.Core
{
    internal class IsDayOffApiClient : IIsDayOffApiClient
    {
        private readonly IHttpClientFactory httpClientFactory;
        private readonly string baseUrl;

        public IsDayOffApiClient(string baseUrl, IHttpClientFactory httpClientFactory)
        {
            this.baseUrl = baseUrl;
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
                    var response = await httpClient.GetAsync(requestUrl, cancellationToken);
                    var responseAsString = await response.Content.ReadAsStringAsync();

                    ValidateResponse(from, to, country, response, responseAsString);

                    return new GetDataApiResponse(responseAsString);
                }
                catch (Exception e)
                {
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

        private void ValidateResponse(DateTime from, DateTime to, Country country, HttpResponseMessage response, string responseContent)
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
        }
        
        private string GetCountryCode(Country country)
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