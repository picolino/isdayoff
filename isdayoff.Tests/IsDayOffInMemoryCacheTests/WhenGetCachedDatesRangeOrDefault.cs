using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Extensions;
using isdayoff.Tests._Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffInMemoryCacheTests
{
    internal class WhenGetCachedDatesRangeOrDefault : IsDayOffInMemoryCacheTestBase
    {
        private Random random;
        
        public override void Setup()
        {
            base.Setup();
            
            random = new Random();
        }

        [Test]
        public async Task ReturnCacheFoundResult([Values] bool cacheHasValue)
        {
            if (cacheHasValue)
            {
                await IsDayOffInMemoryCache.SaveDateRangeInCache(01.01.Of(2020), 01.01.Of(2020), Country.Russia, null,
                                                                 new List<DayOffDateTime> {new DayOffDateTime(01.01.Of(2020), DayType.NotWorkingDay)});
            }
            
            var result = await IsDayOffInMemoryCache.GetCachedDatesRangeOrDefault(01.01.Of(2020), 01.01.Of(2020), Country.Russia, null);
            
            Assert.That(result, cacheHasValue ? Is.Not.Null : Is.Null);
        }

        [Test]
        public async Task IfCacheHasOuterDatesRangeItReturnsNotNullForInnerDatesRangeRequest()
        {
            var cachedFrom = 01.01.Of(2020);
            var cachedTo = 30.01.Of(2020);
            var requestFrom = 10.01.Of(2020);
            var requestTo = 19.01.Of(2020);
            
            await IsDayOffInMemoryCache.SaveDateRangeInCache(cachedFrom, cachedTo, Country.Russia, null, GenerateDayOffDateTimesInDateRange(cachedFrom, cachedTo));
            
            var found = await IsDayOffInMemoryCache.GetCachedDatesRangeOrDefault(requestFrom, requestTo, Country.Russia, null);
            
            Assert.That(found.Count, Is.EqualTo(10));
        }

        [Test]
        public async Task IfCacheNotFullyCoveredRequestedDatesThenCacheReturnsNull()
        {
            var cachedFrom = 01.01.Of(2020);
            var cachedTo = 15.01.Of(2020);
            var requestFrom = 10.01.Of(2020);
            var requestTo = 19.01.Of(2020);
            
            await IsDayOffInMemoryCache.SaveDateRangeInCache(cachedFrom, cachedTo, Country.Russia, null, GenerateDayOffDateTimesInDateRange(cachedFrom, cachedTo));
            
            var found = await IsDayOffInMemoryCache.GetCachedDatesRangeOrDefault(requestFrom, requestTo, Country.Russia, null);
            
            Assert.That(found, Is.Null);
        }

        private List<DayOffDateTime> GenerateDayOffDateTimesInDateRange(DateTime from, DateTime to, DayType dayType = DayType.WorkingDay, bool randomizedDateType = false)
        {
            var allDayTypes = (DayType[]) Enum.GetValues(typeof(DayType));
            return from.ByDaysTill(to).Select(o => new DayOffDateTime(o, randomizedDateType ? allDayTypes[random.Next(allDayTypes.Length)] : dayType)).ToList();
        }
    }
}