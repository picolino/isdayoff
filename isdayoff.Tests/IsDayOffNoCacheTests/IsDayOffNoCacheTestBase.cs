using isdayoff.Core.Cache;

namespace isdayoff.Tests.IsDayOffNoCacheTests
{
    internal class IsDayOffNoCacheTestBase : TestBase
    {
        protected IsDayOffNoCache IsDayOffNoCache { get; private set; }
        
        public override void Setup()
        {
            base.Setup();
            
            IsDayOffNoCache = new IsDayOffNoCache();
        }
    }
}