using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;

namespace isdayoff.Tests._Fakes
{
    public class IsDayOffCacheStub : IIsDayOffCache
    {
        public List<DayOffDateTime> CachedValue { get; set; } = new List<DayOffDateTime>();
        
        public Task SaveDateRangeInCache(DateTime from, DateTime to, Country country, List<DayOffDateTime> dayOffDateTimeList)
        {
            CachedValue = dayOffDateTimeList;
            return Task.CompletedTask;
        }

        public Task<List<DayOffDateTime>> GetCachedDatesRangeOrDefault(DateTime from, DateTime to, Country country)
        {
            return Task.FromResult(CachedValue.Count == 0 ? default : CachedValue);
        }
    }
}