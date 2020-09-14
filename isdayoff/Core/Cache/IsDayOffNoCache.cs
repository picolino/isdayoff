using System.Collections.Generic;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;

namespace isdayoff.Core.Cache
{
    internal class IsDayOffNoCache : IIsDayOffCache
    {
        public void SaveYearInCache(int year, Country country, List<DayOffDateTime> dayOffDateTime)
        {
        }

        public void SaveMonthInCache(int year, int month, Country country, List<DayOffDateTime> dayOffDateTime)
        {
        }

        public void SaveDayInCache(int year, int month, int day, Country country, DayType dayType)
        {
        }

        public bool TryGetCachedYear(int year, Country country, out List<DayOffDateTime> result)
        {
            result = default;
            return false;
        }

        public bool TryGetCachedMonth(int year, int month, Country country, out List<DayOffDateTime> result)
        {
            result = default;
            return false;
        }

        public bool TryGetCachedDay(int year, int month, int day, Country country, out DayType result)
        {
            result = default;
            return false;
        }
    }
}