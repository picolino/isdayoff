using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Responses;

namespace isdayoff.Core
{
    internal interface IIsDayOffApiClient
    {
        Task<GetDataApiResponse> GetDataAsync(int year, DayOffCountry country);
        Task<GetDataApiResponse> GetDataAsync(int year, int month, DayOffCountry country);
        Task<GetDataApiResponse> GetDataAsync(int year, int month, int day, DayOffCountry country);
    }
}