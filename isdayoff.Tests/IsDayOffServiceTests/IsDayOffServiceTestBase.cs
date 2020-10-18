using isdayoff.Core;
namespace isdayoff.Tests.IsDayOffServiceTests
{
    internal class IsDayOffServiceTestBase : TestBase
    {
        protected IsDayOffService IsDayOffService { get; private set; }
        
        public override void Setup()
        {
            base.Setup();
            
            IsDayOffService = new IsDayOffService(ApiClientStub, CacheStub);
        }
    }
}