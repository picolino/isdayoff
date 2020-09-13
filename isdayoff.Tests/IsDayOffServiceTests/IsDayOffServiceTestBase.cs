using isdayoff.Core;
using isdayoff.Tests.Fakes;

namespace isdayoff.Tests.IsDayOffServiceTests
{
    internal class IsDayOffServiceTestBase : TestBase
    {
        protected IsDayOffService IsDayOffService;
        
        public override void Setup()
        {
            base.Setup();
            
            IsDayOffService = new IsDayOffService(IsDayOffSettings.Default, new IsDayOffApiClientStub());
        }
    }
}