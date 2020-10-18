using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;

namespace isdayoff.Tests.Fakes
{
    public class IsDayOffCacheStub : IIsDayOffCache
    {
        public bool HasCachedValue { get; set; }
        public List<DayOffDateTime> CachedValue { get; set; } = new List<DayOffDateTime>();
        
        public Task SaveDateRangeInCache(DateTime from, DateTime to, Country country, List<DayOffDateTime> dayOffDateTimeList)
        {
            CachedValue = dayOffDateTimeList;
            return Task.CompletedTask;
        }

        public Task<bool> TryGetCachedWithinDates(DateTime from, DateTime to, Country country, out List<DayOffDateTime> result)
        {
            result = CachedValue;
            return Task.FromResult(HasCachedValue);
        }
    }
}