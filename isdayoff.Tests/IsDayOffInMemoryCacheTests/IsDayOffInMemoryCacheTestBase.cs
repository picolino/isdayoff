using isdayoff.Core.Cache;

namespace isdayoff.Tests.IsDayOffInMemoryCacheTests
{
    internal class IsDayOffInMemoryCacheTestBase : TestBase
    {
        protected IsDayOffInMemoryCache IsDayOffInMemoryCache { get; private set; }
        
        public override void Setup()
        {
            base.Setup();

            IsDayOffInMemoryCache = new IsDayOffInMemoryCache();
        }
    }
}