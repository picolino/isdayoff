using isdayoff.Core.Http;

namespace isdayoff.Tests.Fakes
{
    internal class HttpClientStubFactory : IHttpClientFactory
    {
        private readonly HttpClientStub httpClient;

        public HttpClientStubFactory(HttpClientStub httpClient)
        {
            this.httpClient = httpClient;
        }
        
        public IHttpClient CreateHttpClient()
        {
            return httpClient;
        }
    }
}