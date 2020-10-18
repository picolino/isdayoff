using isdayoff.Core;

namespace isdayoff.Tests.IsDayOffApiClientTests
{
    internal class IsDayOffApiClientTestBase : TestBase
    {
        protected const string ApiBaseUrl = "https://dev.isdayoff.ru/api/";
        protected IsDayOffApiClient IsDayOffApiClient { get; private set; }

        public override void Setup()
        {
            base.Setup();
            
            IsDayOffApiClient = new IsDayOffApiClient(ApiBaseUrl, HttpClientStubFactory);
        }
    }
}