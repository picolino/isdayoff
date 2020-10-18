using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using isdayoff.Core.Exceptions;
using isdayoff.Core.Extensions;

namespace isdayoff.Core
{
    internal class IsDayOffService
    {
        private readonly IIsDayOffApiClient apiClient;
        private readonly IIsDayOffCache cache;
        
        public IsDayOffService(IIsDayOffApiClient apiClient, IIsDayOffCache cache)
        {
            this.apiClient = apiClient;
            this.cache = cache;
        }

        public async Task<List<DayOffDateTime>> CheckDatesRangeAsync(DateTime from, DateTime to, Country country, CancellationToken cancellationToken)
        {
            if (from > to)
            {
                var temp = to;
                to = from;
                from = temp;
            }
            
            if (await cache.TryGetCachedDatesRange(from, to, country, out var cachedResult))
            {
                return cachedResult;
            }
                
            var response = await apiClient.GetDataAsync(from, to, country, cancellationToken);
            var result = GenerateDayOffDateTimeList(response.Result, from.ByDaysTill(to).ToList());

            await cache.SaveDateRangeInCache(from, to, country, result);

            return result;
        }

        private static List<DayOffDateTime> GenerateDayOffDateTimeList(string responseResult, IReadOnlyList<DateTime> dates)
        {
            var results = new DayOffDateTime[dates.Count];
            
            for (var i = 0; i < dates.Count; i++)
            {
                var day = dates[i];
                var charDayRepresentation = responseResult[i];
                var dayType = ConvertCharToDateType(charDayRepresentation);
                results[i] = new DayOffDateTime(day, dayType);
            }

            return results.ToList();
        }

        private static DayType ConvertCharToDateType(char character)
        {
            switch (character)
            {
                case '0':
                    return DayType.WorkingDay;
                case '1':
                    return DayType.NotWorkingDay;
                case '2':
                    return DayType.ShortDay;
                case '4':
                    return DayType.WorkingDayAdvanced;
                default:
                    throw new ArgumentOutOfRangeException(nameof(character), character, ErrorsMessages.UnknownResult());
            }
        }
    }
}