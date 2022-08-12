using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace isdayoff.Contract.Abstractions
{
    /// <summary>
    /// You can use this interface to create your own cache implementation.
    /// </summary>
    public interface IIsDayOffCache
    {
        Task SaveDateRangeInCache(DateTime from, DateTime to, Country country, Region? region, List<DayOffDateTime> dayOffDateTimeList);
        Task<List<DayOffDateTime>> GetCachedDatesRangeOrDefault(DateTime from, DateTime to, Country country, Region? region);
    }
}