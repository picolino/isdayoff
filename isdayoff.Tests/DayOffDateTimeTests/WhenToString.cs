using isdayoff.Contract;
using isdayoff.Tests._Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.DayOffDateTimeTests
{
    internal class WhenToString : TestBase
    {
        [Test]
        public void StringContainsDayOffInfoWithDateTime([Values] DayType dayType)
        {
            var dateTime = 04.08.Of(2020);
            
            var dayOffInfo = new DayOffDateTime(dateTime, dayType);
            var dayOffInfoAsString = dayOffInfo.ToString();
            
            Assert.That(dayOffInfoAsString, Is.EqualTo($"{dateTime:d} ({dayType:G})"));
        }
    }
}