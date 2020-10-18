namespace isdayoff.Core.Http
{
    internal class HttpClientFactory : IHttpClientFactory
    {
        public IHttpClient CreateHttpClient()
        {
            return new HttpClient();
        }
    }
}