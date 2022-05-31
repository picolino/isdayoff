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

        public async Task<List<DayOffDateTime>> CheckDatesRangeAsync(
            IsDayOffGetDatesRangeArgs args, 
            CancellationToken cancellationToken)
        {
            var result = await cache.GetCachedDatesRangeOrDefault(args.From, args.To, args.Country).ConfigureAwait(false);
            
            if (result is null)
            {
                var response = await apiClient.GetDataAsync(
                                   args.From, 
                                   args.To, 
                                   args.Country, 
                                   args.UseShortDays,
                                   args.TreatNonWorkingDaysByCovidAsWorkingDayAdvanced,
                                   args.UseSixDaysWorkWeek,
                                   cancellationToken).ConfigureAwait(false);
                
                result = GenerateDayOffDateTimeList(response.Result, args.From.ByDaysTill(args.To).ToList());

                await cache.SaveDateRangeInCache(args.From, args.To, args.Country, result).ConfigureAwait(false);
            }
            
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
                    throw new ArgumentOutOfRangeException(nameof(character), character, ErrorsMessages.UnknownResponseDayType());
            }
        }
    }
}