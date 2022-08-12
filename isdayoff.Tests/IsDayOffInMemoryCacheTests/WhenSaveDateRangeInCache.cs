using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Tests._Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffInMemoryCacheTests
{
    internal class WhenSaveDateRangeInCache : IsDayOffInMemoryCacheTestBase
    {
        [Test]
        public async Task NoExceptionOccurs([Values] Country country, [Values] Region? region)
        {
            await IsDayOffInMemoryCache.SaveDateRangeInCache(01.01.Of(2020),  01.01.Of(2020), country, region, new List<DayOffDateTime>{new DayOffDateTime(01.01.Of(2020), DayType.NotWorkingDay)});
            Assert.Pass();
        }
    }
}