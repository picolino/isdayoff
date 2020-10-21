using isdayoff.Contract;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.DayOffDateTimeTests
{
    internal class WhenConstructs : TestBase
    {
        [Test]
        public void DateTimeInfoSaves()
        {
            var dayOffInfo = new DayOffDateTime(04.08.Of(2020), DayType.ShortDay);

            Assert.That(dayOffInfo.DateTime, Is.EqualTo(04.08.Of(2020)));
        }
        
        [Test]
        public void DayTypeInfoSaves([Values] DayType dayType)
        {
            var dayOffInfo = new DayOffDateTime(04.08.Of(2020), dayType);

            Assert.That(dayOffInfo.DayType, Is.EqualTo(dayType));
        }
    }
}