using isdayoff.Core.Http;

namespace isdayoff.Tests.HttpClientFactoryTests
{
    internal class HttpClientFactoryTestBase : TestBase
    {
        protected HttpClientFactory HttpClientFactory { get; private set; }
        
        public override void Setup()
        {
            base.Setup();
            
            HttpClientFactory = new HttpClientFactory(HttpMessageHandlerMock);
        }
    }
}