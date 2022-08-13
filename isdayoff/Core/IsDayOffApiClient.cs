using System;
using System.Collections.Generic;
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
        
        private Dictionary<Country, string> countryCodes = new Dictionary<Country, string>
        {
            { Country.Russia, "ru" },
            { Country.Belarus, "by" },
            { Country.Ukraine, "ua" },
            { Country.Kazakhstan, "kz" },
            { Country.USA, "us" },
            { Country.Uzbekistan, "uz" },
            { Country.Turkey, "tr" },
            { Country.Latvia, "lv" },
        };
        
        private Dictionary<Region, string> regionCodes = new Dictionary<Region, string>
        {
            { Region.RuAd, "ad" },
            { Region.RuAl, "al" },
            { Region.RuBa, "ba" },
            { Region.RuBel, "bel" },
            { Region.RuBu, "bu" },
            { Region.RuCe, "ce" },
            { Region.RuCu, "cu" },
            { Region.RuDa, "da" },
            { Region.RuIn, "in" },
            { Region.RuKa, "ka" },
            { Region.RuKb, "kb" },
            { Region.RuKc, "kc" },
            { Region.RuKl, "kl" },
            { Region.RuPnz, "pnz" },
            { Region.RuSa, "sa" },
            { Region.RuSar, "sar" },
            { Region.RuSe, "se" },
            { Region.RuSta, "sta" },
            { Region.RuTa, "ta" },
            { Region.RuTy, "ty" },
            { Region.RuZab, "zab" },
        };

        public IsDayOffApiClient(string baseUrl, string userAgent, HttpClientFactory httpClientFactory)
        {
            this.baseUrl = baseUrl;
            this.userAgent = userAgent;
            this.httpClientFactory = httpClientFactory;
        }

        public async Task<GetDataApiResponse> GetDataAsync(
            DateTime from, 
            DateTime to, 
            Country country,
            Region? region,
            bool useShortDays,
            bool treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
            bool useSixDaysWorkWeek, 
            CancellationToken cancellationToken)
        {
            return await GetDataInternalAsync(
                       from, 
                       to, 
                       country, 
                       region,
                       useShortDays, 
                       treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
                       useSixDaysWorkWeek, 
                       cancellationToken).ConfigureAwait(false);
        }
        
        private async Task<GetDataApiResponse> GetDataInternalAsync(
            DateTime from, 
            DateTime to, 
            Country country,
            Region? region,
            bool useShortDays,
            bool treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
            bool useSixDaysWorkWeek, 
            CancellationToken cancellationToken)
        {
            using (var httpClient = httpClientFactory.CreateHttpClient())
            {
                var countryCode = GetCountryCode(country);
                var regionCode = GetRegionCode(region);
                
                var requestUrl = BuildGetDataRequestUrl(
                    from, 
                    to, 
                    countryCode, 
                    regionCode,
                    useShortDays, 
                    treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
                    useSixDaysWorkWeek);

                try
                {
                    httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

                    IsDayOff.Tracer.TraceEvent(TraceEventType.Information, TraceEventIds.Requesting.REQUEST_SENDING,
                                               "Sending HTTP GET: '{0}'", requestUrl);
                    
                    var response = await httpClient.GetAsync(requestUrl, cancellationToken).ConfigureAwait(false);
                    var responseAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

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

        private string BuildGetDataRequestUrl(
            DateTime from, 
            DateTime to, 
            string countryCode,
            string regionCode,
            bool useShortDays,
            bool treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
            bool useSixDaysWorkWeek)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(baseUrl);
            stringBuilder.Append("getdata");
            stringBuilder.Append("?date1=");
            stringBuilder.AppendFormat("{0:yyyyMMdd}", from);
            stringBuilder.Append("&date2=");
            stringBuilder.AppendFormat("{0:yyyyMMdd}", to);
            stringBuilder.Append("&cc=");
            stringBuilder.Append(string.IsNullOrWhiteSpace(regionCode) ? countryCode : string.Join("-", countryCode, regionCode));
            stringBuilder.Append("&pre=");
            stringBuilder.Append(useShortDays ? 1 : 0);
            stringBuilder.Append("&covid=");
            stringBuilder.Append(treatNonWorkingDaysByCovidAsWorkingDayAdvanced ? 1 : 0);
            stringBuilder.Append("&sd=");
            stringBuilder.Append(useSixDaysWorkWeek ? 1 : 0);

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
        
        private string GetCountryCode(Country country)
        {
            if (countryCodes.TryGetValue(country, out var countryCode))
            {
                return countryCode;
            }
            
            throw new ArgumentOutOfRangeException(nameof(country), country, ErrorsMessages.UnknownCountry());
        }

        private string GetRegionCode(Region? region)
        {
            string regionCodeResult;
            
            if (region.HasValue)
            {
                if (regionCodes.TryGetValue(region.Value, out var regionCode))
                {
                    regionCodeResult = regionCode;
                }
                else
                {
                    throw new ArgumentOutOfRangeException(nameof(region), region, ErrorsMessages.UnknownRegion());
                }
            }
            else
            {
                regionCodeResult = string.Empty;
            }

            return regionCodeResult;
        }
    }
}