using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Tests._Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffServiceTests
{
    internal class WhenCheckDatesRange : IsDayOffServiceTestBase
    {
        [Test]
        public async Task IfCachedValueExistsThenCachedValueReturns()
        {
            CacheStub.CachedValue.Add(new DayOffDateTime(01.01.Of(2020), DayType.NotWorkingDay));
            
            var result = await IsDayOffService.CheckDatesRangeAsync(01.01.Of(2020), 01.01.Of(2020), Country.Russia, CancellationToken.None);

            Assert.That(result.Single().DayType, Is.EqualTo(DayType.NotWorkingDay));
        }

        [Test]
        public async Task IfNoCachedValueExistsThenApiResponseReturns()
        {
            ApiClientStub.Response = "1";
            
            var result = await IsDayOffService.CheckDatesRangeAsync(04.08.Of(2020), 04.08.Of(2020), Country.Russia, CancellationToken.None);

            Assert.That(result.Single().DayType, Is.EqualTo(DayType.NotWorkingDay));
        }

        [Test]
        [TestCase(0, DayType.WorkingDay)]
        [TestCase(1, DayType.NotWorkingDay)]
        [TestCase(2, DayType.ShortDay)]
        [TestCase(4, DayType.WorkingDayAdvanced)]
        public async Task ApiResponseMapsToDayTypeCorrectly(int apiResponse, DayType expectedDateType)
        {
            ApiClientStub.Response = apiResponse.ToString();
            
            var result = await IsDayOffService.CheckDatesRangeAsync(04.08.Of(2020), 04.08.Of(2020), Country.Russia, CancellationToken.None);

            Assert.That(result.Single().DayType, Is.EqualTo(expectedDateType));
        }

        [Test]
        public void IfResponseTypeIsUnknownThenArgumentExceptionThrows([Values("5", "6", "7", "8", "9", "56789", "0010as", "05", "a01")] string apiResponse)
        {
            var initialDate = 04.08.Of(2020);
            var endDate = initialDate.AddDays(apiResponse.Length);
            ApiClientStub.Response = apiResponse;

            async Task Act()
            {
                await IsDayOffService.CheckDatesRangeAsync(initialDate, endDate, Country.Russia, CancellationToken.None);
            } 
            
            Assert.ThrowsAsync<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        public async Task ApiResponseReturnsSameDateTimeAsProvided()
        {
            var result = await IsDayOffService.CheckDatesRangeAsync(04.08.Of(2020), 04.08.Of(2020), Country.Russia, CancellationToken.None);

            Assert.That(result.Single().DateTime, Is.EqualTo(04.08.Of(2020)));
        }

        [Test]
        public async Task IfDateFromGreaterThanDateToTheySwapsAutomatically()
        {
            ApiClientStub.Response = "00";
            var from = 06.08.Of(2020);
            var to = 05.08.Of(2020);
            
            var result = await IsDayOffService.CheckDatesRangeAsync(from, to, Country.Russia, CancellationToken.None);

            Assert.That(result[0].DateTime == to);
            Assert.That(result[1].DateTime == from);
        }
    }
}