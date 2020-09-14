using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core;
using isdayoff.Core.Responses;

namespace isdayoff.Tests.Fakes
{
    public class IsDayOffApiClientStub : IIsDayOffApiClient
    {
        public string Response { get; set; }
        
        public Task<GetDataApiResponse> GetDataAsync(int year, Country country)
        {
            return Task.FromResult(new GetDataApiResponse(Response));
        }

        public Task<GetDataApiResponse> GetDataAsync(int year, int month, Country country)
        {
            return Task.FromResult(new GetDataApiResponse(Response));
        }

        public Task<GetDataApiResponse> GetDataAsync(int year, int month, int day, Country country)
        {
            return Task.FromResult(new GetDataApiResponse(Response));
        }
    }
}