using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace isdayoff.Contract.Abstractions
{
    /// <summary>
    /// You can use this interface to create your own cache implementation.
    /// </summary>
    [PublicAPI]
    public interface IIsDayOffCache
    {
        Task SaveDateRangeInCache(DateTime from, DateTime to, Country country, List<DayOffDateTime> dayOffDateTimeList);
        Task<bool> TryGetCachedWithinDates(DateTime from, DateTime to, Country country, out List<DayOffDateTime> result);
    }
}