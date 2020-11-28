using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Extensions;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffInMemoryCacheTests
{
    internal class WhenTryGetCachedDatesRange : IsDayOffInMemoryCacheTestBase
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
                await IsDayOffInMemoryCache.SaveDateRangeInCache(01.01.Of(2020), 01.01.Of(2020), Country.Russia, 
                                                                 new List<DayOffDateTime> {new DayOffDateTime(01.01.Of(2020), DayType.NotWorkingDay)});
            }
            
            var found = await IsDayOffInMemoryCache.TryGetCachedDatesRange(01.01.Of(2020), 01.01.Of(2020), Country.Russia, out _);
            
            Assert.That(found, Is.EqualTo(cacheHasValue));
        }

        [Test]
        public async Task IfCacheHasOuterDatesRangeItReturnsTrueForInnerDatesRangeRequest()
        {
            var cachedFrom = 01.01.Of(2020);
            var cachedTo = 30.01.Of(2020);
            var requestFrom = 10.01.Of(2020);
            var requestTo = 19.01.Of(2020);
            
            await IsDayOffInMemoryCache.SaveDateRangeInCache(cachedFrom, cachedTo, Country.Russia, GenerateDayOffDateTimesInDateRange(cachedFrom, cachedTo));
            
            var found = await IsDayOffInMemoryCache.TryGetCachedDatesRange(requestFrom, requestTo, Country.Russia, out var result);
            
            Assert.That(found, Is.EqualTo(true));
            Assert.That(result.Count, Is.EqualTo(10));
        }

        [Test]
        public async Task IfCacheNotFullyCoveredRequestedDatesThenCacheReturnsFalse()
        {
            var cachedFrom = 01.01.Of(2020);
            var cachedTo = 15.01.Of(2020);
            var requestFrom = 10.01.Of(2020);
            var requestTo = 19.01.Of(2020);
            
            await IsDayOffInMemoryCache.SaveDateRangeInCache(cachedFrom, cachedTo, Country.Russia, GenerateDayOffDateTimesInDateRange(cachedFrom, cachedTo));
            
            var found = await IsDayOffInMemoryCache.TryGetCachedDatesRange(requestFrom, requestTo, Country.Russia, out _);
            
            Assert.That(found, Is.EqualTo(false));
        }

        private List<DayOffDateTime> GenerateDayOffDateTimesInDateRange(DateTime from, DateTime to, DayType dayType = DayType.WorkingDay, bool randomizedDateType = false)
        {
            var allDayTypes = (DayType[]) Enum.GetValues(typeof(DayType));
            return from.ByDaysTill(to).Select(o => new DayOffDateTime(o, randomizedDateType ? allDayTypes[random.Next(allDayTypes.Length)] : dayType)).ToList();
        }
    }
}