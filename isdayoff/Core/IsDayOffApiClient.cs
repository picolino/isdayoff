using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Exceptions;
using isdayoff.Core.Responses;

namespace isdayoff.Core
{
    internal class IsDayOffApiClient : IIsDayOffApiClient
    {
        private readonly string baseUrl;

        public IsDayOffApiClient(string baseUrl)
        {
            this.baseUrl = baseUrl;
        }
        
        public async Task<GetDataApiResponse> GetDataAsync(int year, Country country)
        {
            return await GetDataInternalAsync(year, null, null, country);
        }
        
        public async Task<GetDataApiResponse> GetDataAsync(int year, int month, Country country)
        {
            return await GetDataInternalAsync(year, month, null, country);
        }
        
        public async Task<GetDataApiResponse> GetDataAsync(int year, int month, int day, Country country)
        {
            return await GetDataInternalAsync(year, month, day, country);
        }
        
        private async Task<GetDataApiResponse> GetDataInternalAsync(int year, int? month, int? day, Country country)
        {
            using (var httpClient = new HttpClient())
            {
                var requestUrl = BuildGetDataRequestUrl(year, month, day, country);

                try
                {
                    var response = await httpClient.GetAsync(requestUrl);
                    
                    response.EnsureSuccessStatusCode();
                    
                    var responseAsString = await response.Content.ReadAsStringAsync();
                    return new GetDataApiResponse(responseAsString);
                }
                catch (Exception e)
                {
                    throw new IsDayOffExternalServiceException("Something wrong happened while processing request to isdayoff external service. See details in inner exception.", e);
                }
            }
        }

        private string BuildGetDataRequestUrl(int year, int? month, int? day, Country country)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(baseUrl);
            stringBuilder.Append("getdata?year=");
            stringBuilder.AppendFormat("{0:0000}", year);

            if (month.HasValue)
            {
                stringBuilder.Append("&month=");
                stringBuilder.AppendFormat("{0:00}", month);
            }

            if (day.HasValue)
            {
                stringBuilder.Append("&day=");
                stringBuilder.AppendFormat("{0:00}", day);
            }

            stringBuilder.Append("&cc=");
            stringBuilder.Append(GetCountryCode(country));

            return stringBuilder.ToString();
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
                    throw new ArgumentOutOfRangeException(nameof(country), country, "Unknown country");
            }
        }
    }
}