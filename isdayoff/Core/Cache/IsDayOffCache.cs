using System;
using System.Collections.Generic;
using isdayoff.Contract;

namespace isdayoff.Core.Cache
{
    internal class IsDayOffCache
    {
        public IsDayOffCache()
        {
            YearsCache = new Dictionary<string, List<DayOffDateTime>>();
            MonthsCache = new Dictionary<string, List<DayOffDateTime>>();
            DaysCache = new Dictionary<string, DayType>();
        }
        
        public Dictionary<string, List<DayOffDateTime>> YearsCache { get; }
        public Dictionary<string, List<DayOffDateTime>> MonthsCache { get; }
        public Dictionary<string, DayType> DaysCache { get; }

        public void SaveYearInCache(int year, DayOffCountry country, List<DayOffDateTime> dayOffDateTime)
        {
            var key = BuildKey(country, year);
            YearsCache[key] = dayOffDateTime;
        }

        public void SaveMonthInCache(int year, int month, DayOffCountry country, List<DayOffDateTime> dayOffDateTime)
        {
            var key = BuildKey(country, year, month);
            MonthsCache[key] = dayOffDateTime;
        }

        public void SaveDayInCache(int year, int month, int day, DayOffCountry country, DayType dayType)
        {
            var key = BuildKey(country, year, month, day);
            DaysCache[key] = dayType;
        }

        public bool TryGetCachedYear(int year, DayOffCountry country, out List<DayOffDateTime> result)
        {
            var key = BuildKey(country, year);
            
            if (YearsCache.ContainsKey(key))
            {
                result = YearsCache[key];
                return true;
            }

            result = null;

            return false;
        }

        public bool TryGetCachedMonth(int year, int month, DayOffCountry country, out List<DayOffDateTime> result)
        {
            var key = BuildKey(country, year, month);
            
            if (MonthsCache.ContainsKey(key))
            {
                result = MonthsCache[key];
                return true;
            }

            result = null;

            return false;
        }

        public bool TryGetCachedDay(int year, int month, int day, DayOffCountry country, out DayType result)
        {
            var key = BuildKey(country, year, month, day);
            
            if (DaysCache.ContainsKey(key))
            {
                result = DaysCache[key];
                return true;
            }

            result = default;

            return false;
        }

        private static string BuildKey(DayOffCountry country, int year, int? month = null, int? day = null)
        {
            var key = year;
            
            if (month.HasValue)
            {
                key = key * 100 + month.Value;
            }

            if (day.HasValue)
            {
                key = key * 100 + day.Value;
            }

            return $"{country:G}-{key}";
        }
    }
}