using System;
using System.Collections;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffNoCacheTests
{
    internal class WhenTryGetCachedDatesRange : IsDayOffNoCacheTestBase
    {
        [Test]
        [TestCaseSource(nameof(RandomCacheRequestsTestData))]
        public async Task FalseAlwaysReturns(DateTime from, DateTime to, Country country)
        {
            var cacheFound = await IsDayOffNoCache.TryGetCachedDatesRange(from, to, country, out _);

            Assert.That(cacheFound, Is.EqualTo(false));
        }

        [Test]
        [TestCaseSource(nameof(RandomCacheRequestsTestData))]
        public async Task NullListAlwaysReturns(DateTime from, DateTime to, Country country)
        {
            await IsDayOffNoCache.TryGetCachedDatesRange(from, to, country, out var cacheResultList);

            Assert.That(cacheResultList, Is.Null);
        }
        
        private static IEnumerable RandomCacheRequestsTestData()
        {
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia);
            yield return new TestCaseData(04.08.Of(2020), 05.08.Of(2020), Country.Russia);
            yield return new TestCaseData(31.08.Of(2020), 05.09.Of(2020), Country.Russia);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Belarus);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Kazakhstan);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Ukraine);
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.USA);
        }
    }
}