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

        public Task<List<DayOffDateTime>> GetCachedDatesRangeOrDefault(DateTime from, DateTime to, Country country)
        {
            return Task.FromResult(default(List<DayOffDateTime>));
        }
    }
}