using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Contract.Abstractions;
using isdayoff.Core.Extensions;
using isdayoff.Core.Tracing;

namespace isdayoff.Core.Cache
{
    internal class IsDayOffInMemoryCache : IIsDayOffCache
    {
        private readonly object locker = new object();
        
        public IsDayOffInMemoryCache()
        {
            Cache = new SortedList<(Country, Region?, DateTime), DayOffDateTime>();
        }

        private SortedList<(Country, Region?, DateTime), DayOffDateTime> Cache { get; }

        public Task SaveDateRangeInCache(DateTime from, DateTime to, Country country, Region? region, List<DayOffDateTime> dayOffDateTimeList)
        {
            lock (locker)
            {
                foreach (var dayOffDateTime in dayOffDateTimeList)
                {
                    Cache[(country, region, dayOffDateTime.DateTime)] = dayOffDateTime;
                }
                
                IsDayOff.Tracer.TraceEvent(TraceEventType.Information, TraceEventIds.Caching.CACHE_SAVE_VALUE,
                                           "Day off info of '{0}' days from '{1}' to '{2}' for '{3}' saved into in-memory cache", dayOffDateTimeList.Count, from, to, country);

                return Task.CompletedTask;
            }
        }

        public Task<List<DayOffDateTime>> GetCachedDatesRangeOrDefault(DateTime from, DateTime to, Country country, Region? region)
        {
            var result = new List<DayOffDateTime>();

            var days = from.ByDaysTill(to).ToList();

            lock (locker)
            {
                var notFoundInCache = false;
                
                foreach (var dateTime in days)
                {
                    if (Cache.TryGetValue((country, region, dateTime), out var cachedDayOffDateTime))
                    {
                        result.Add(cachedDayOffDateTime);
                    }
                    else
                    {
                        result = default;
                        notFoundInCache = true;
                        break;
                    }
                }

                if (notFoundInCache)
                {
                    IsDayOff.Tracer.TraceEvent(TraceEventType.Information, TraceEventIds.Caching.CACHE_LOAD_VALUE_NOT_FOUND,
                                               "Day off info from '{0}' to '{1}' of '{2}' not found in in-memory cache", from, to, country);
                }
                else
                {
                    IsDayOff.Tracer.TraceEvent(TraceEventType.Information, TraceEventIds.Caching.CACHE_LOAD_VALUE_FOUND,
                                               "Day off info from '{0}' to '{1}' of '{2}' found in in-memory cache", from, to, country);
                }
            }

            return Task.FromResult(result);
        }
    }
}