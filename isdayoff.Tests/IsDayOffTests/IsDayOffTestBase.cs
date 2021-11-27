using System.Diagnostics;
using isdayoff.Contract;

namespace isdayoff.Tests.IsDayOffTests
{
    internal class IsDayOffTestBase : TestBase
    {
        protected IsDayOff IsDayOff { get; private set; }

        public override void Setup()
        {
            base.Setup();

            var settings = new IsDayOffSettings(
                ApiBaseUrlStub,
                UserAgentStub,
                CacheStub,
                Country.Russia,
                false,
                false,
                false,
                SourceLevels.Off);
            IsDayOff = new IsDayOff(settings, ApiClientStub);
        }
    }
}