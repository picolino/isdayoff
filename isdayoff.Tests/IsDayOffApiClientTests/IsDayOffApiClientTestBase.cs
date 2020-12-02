using isdayoff.Core;
using isdayoff.Core.Http;

namespace isdayoff.Tests.IsDayOffApiClientTests
{
    internal class IsDayOffApiClientTestBase : TestBase
    {
        protected IsDayOffApiClient IsDayOffApiClient { get; private set; }

        public override void Setup() 
        {
            base.Setup();
            
            IsDayOffApiClient = new IsDayOffApiClient(ApiBaseUrlStub, UserAgentStub, new HttpClientFactory(HttpMessageHandlerMock));
        }
    }
}