using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace isdayoff.Core.Http
{
    internal class HttpClient : IHttpClient
    {
        private readonly System.Net.Http.HttpClient httpClient;
        
        public HttpClient()
        {
            httpClient = new System.Net.Http.HttpClient();
        }

        public async Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken)
        {
            return await httpClient.GetAsync(url, cancellationToken);
        }
        
        public void Dispose()
        {
            httpClient.Dispose();
        }
    }
}