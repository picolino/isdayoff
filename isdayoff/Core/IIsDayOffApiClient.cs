using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Responses;

namespace isdayoff.Core
{
    internal interface IIsDayOffApiClient
    {
        Task<GetDataApiResponse> GetDataAsync(int year, Country country);
        Task<GetDataApiResponse> GetDataAsync(int year, int month, Country country);
        Task<GetDataApiResponse> GetDataAsync(int year, int month, int day, Country country);
    }
}