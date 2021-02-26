using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffTests
{
    internal class WhenCheckMonth : IsDayOffTestBase
    {
        [Test]
        public void NoExceptionShouldBeThrown()
        {
            var response = new string(Enumerable.Range(0, 31).Select(o => '0').ToArray());
            ApiClientStub.Response = response;
            
            async Task Act()
            {
                await IsDayOff.CheckMonthAsync(2020, 01);
            }

            Assert.DoesNotThrowAsync(Act);
        }
    }
}