using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;

namespace isdayoff.Core.Cache
{
    internal class IsDayOffNoCache : IIsDayOffCache
    {
        public Task SaveDateRangeInCache(DateTime from, DateTime to, Country country, List<DayOffDateTime> dayOffDateTimeList)
        {
            return Task.CompletedTask;
        }

        public Task<bool> TryGetCachedDatesRange(DateTime from, DateTime to, Country country, out List<DayOffDateTime> result)
        {
            result = default;
            return Task.FromResult(false);
        }
    }
}