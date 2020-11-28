using System.Collections.Generic;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffInMemoryCacheTests
{
    internal class WhenSaveDateRangeInCache : IsDayOffInMemoryCacheTestBase
    {
        [Test]
        public async Task NoExceptionOccurs([Values] Country country)
        {
            await IsDayOffInMemoryCache.SaveDateRangeInCache(01.01.Of(2020),  01.01.Of(2020), country, new List<DayOffDateTime>{new DayOffDateTime(01.01.Of(2020), DayType.NotWorkingDay)});
            Assert.Pass();
        }
    }
}