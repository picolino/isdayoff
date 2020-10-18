using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using isdayoff.Core.Extensions;

namespace isdayoff.Core.Cache
{
    internal class IsDayOffInMemoryCache : IIsDayOffCache
    {
        private readonly object locker = new object();
        
        public IsDayOffInMemoryCache()
        {
            Cache = new SortedList<(Country, DateTime), DayOffDateTime>();
        }

        private SortedList<(Country, DateTime), DayOffDateTime> Cache { get; }

        public Task SaveDateRangeInCache(DateTime from, DateTime to, Country country, List<DayOffDateTime> dayOffDateTimeList)
        {
            lock (locker)
            {
                foreach (var dayOffDateTime in dayOffDateTimeList)
                {
                    Cache[(country, dayOffDateTime.DateTime)] = dayOffDateTime;
                }

                return Task.CompletedTask;
            }
        }

        public Task<bool> TryGetCachedWithinDates(DateTime from, DateTime to, Country country, out List<DayOffDateTime> result)
        {
            result = new List<DayOffDateTime>();
            
            var hasInCache = true;

            var days = from.ByDaysTill(to).ToList();

            lock (locker)
            {
                foreach (var dateTime in days)
                {
                    if (Cache.TryGetValue((country, dateTime), out var cachedDayOffDateTime))
                    {
                        result.Add(cachedDayOffDateTime);
                    }
                    else
                    {
                        hasInCache = false;
                        result = default;
                        break;
                    }
                }
            }

            return Task.FromResult(hasInCache);
        }
    }
}