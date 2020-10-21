using isdayoff.Core.Cache;

namespace isdayoff.Tests.IsDayOffInMemoryCacheTests
{
    internal class IsDayOffInMemoryTestBase : TestBase
    {
        protected IsDayOffInMemoryCache IsDayOffInMemoryCache { get; private set; }
        
        public override void Setup()
        {
            base.Setup();

            IsDayOffInMemoryCache = new IsDayOffInMemoryCache();
        }
    }
}