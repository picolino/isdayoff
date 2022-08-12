using System;
using System.Collections;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Tests._Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffNoCacheTests
{
    internal class WhenTryGetCachedDatesRange : IsDayOffNoCacheTestBase
    {
        [Test]
        [TestCaseSource(nameof(RandomCacheRequestsTestData))]
        public async Task NullAlwaysReturns(DateTime from, DateTime to, Country country, Region? region)
        {
            var cacheFound = await IsDayOffNoCache.GetCachedDatesRangeOrDefault(from, to, country, region);

            Assert.That(cacheFound, Is.Null);
        }
        
        private static IEnumerable RandomCacheRequestsTestData()
        {
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, null);
            yield return new TestCaseData(04.08.Of(2020), 05.08.Of(2020), Country.Russia, null);
            yield return new TestCaseData(31.08.Of(2020), 05.09.Of(2020), Country.Russia, null);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, Region.RuAd);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Belarus, null);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Kazakhstan, null);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Ukraine, null);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.USA, null);
        }
    }
}