using isdayoff.Contract;

namespace isdayoff.Tests.IsDayOffSettingsTests
{
    internal class IsDayOffSettingsTestBase : TestBase
    {
        protected IsDayOffSettings IsDayOffSettings { get; private set; }
        
        public override void Setup()
        {
            base.Setup();
            
            IsDayOffSettings = new IsDayOffSettings(CacheStub, Country.Russia);
        }
    }
}