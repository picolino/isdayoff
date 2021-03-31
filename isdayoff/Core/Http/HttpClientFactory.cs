using System.Net.Http;

namespace isdayoff.Core.Http
{
    internal class HttpClientFactory
    {
        private readonly HttpMessageHandler httpClientHandler;

        public HttpClientFactory(HttpMessageHandler httpClientHandler)
        {
            this.httpClientHandler = httpClientHandler;
        }
        
        public HttpClient CreateHttpClient()
        {
            return new HttpClient(httpClientHandler, false);
        }
    }
}