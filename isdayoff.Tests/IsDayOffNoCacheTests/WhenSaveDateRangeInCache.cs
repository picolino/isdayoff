using System.Collections.Generic;
using isdayoff.Contract;
using isdayoff.Tests._Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffNoCacheTests
{
    internal class WhenSaveDateRangeInCache : IsDayOffNoCacheTestBase
    {
        [Test]
        public void NothingHappens()
        {
            IsDayOffNoCache.SaveDateRangeInCache(04.08.Of(2020), 04.08.Of(2020), Country.Russia, null, new List<DayOffDateTime>());
            
            Assert.Pass();
        }
    }
}