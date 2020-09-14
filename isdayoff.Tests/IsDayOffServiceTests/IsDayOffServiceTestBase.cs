using isdayoff.Core;
using isdayoff.Core.Cache;
using isdayoff.Tests.Fakes;

namespace isdayoff.Tests.IsDayOffServiceTests
{
    internal class IsDayOffServiceTestBase : TestBase
    {
        protected IsDayOffService IsDayOffService;
        
        public override void Setup()
        {
            base.Setup();
            
            IsDayOffService = new IsDayOffService(new IsDayOffApiClientStub(), new IsDayOffNoCache());
        }
    }
}