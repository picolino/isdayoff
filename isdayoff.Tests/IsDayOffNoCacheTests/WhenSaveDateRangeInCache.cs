using System.Collections.Generic;
using isdayoff.Contract;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffNoCacheTests
{
    internal class WhenSaveDateRangeInCache : IsDayOffNoCacheTestBase
    {
        [Test]
        public void NothingHappens()
        {
            IsDayOffNoCache.SaveDateRangeInCache(04.08.Of(2020), 04.08.Of(2020), Country.Russia, new List<DayOffDateTime>());
            
            Assert.Pass();
        }
    }
}