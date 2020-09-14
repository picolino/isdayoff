using System.Collections.Generic;

namespace isdayoff.Contract.Abstractions
{
    public interface IIsDayOffCache
    {
        void SaveYearInCache(int year, Country country, List<DayOffDateTime> dayOffDateTime);
        void SaveMonthInCache(int year, int month, Country country, List<DayOffDateTime> dayOffDateTime);
        void SaveDayInCache(int year, int month, int day, Country country, DayType dayType);
        bool TryGetCachedYear(int year, Country country, out List<DayOffDateTime> result);
        bool TryGetCachedMonth(int year, int month, Country country, out List<DayOffDateTime> result);
        bool TryGetCachedDay(int year, int month, int day, Country country, out DayType result);
    }
}