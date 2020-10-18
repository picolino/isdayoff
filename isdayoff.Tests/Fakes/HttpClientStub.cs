using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Core.Http;

namespace isdayoff.Tests.Fakes
{
    internal class HttpClientStub : IHttpClient
    {
        public string ResponseStringContent { get; set; } = "0";
        public HttpStatusCode ResponseStatusCode { get; set; } = HttpStatusCode.OK;
        
        public string LatestGetRequestUrl { get; private set; }
        
        public Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken)
        {
            LatestGetRequestUrl = url;
            
            return Task.FromResult(new HttpResponseMessage(ResponseStatusCode)
                                   {
                                       Content = new StringContent(ResponseStringContent)
                                   });
        }
        
        public void Dispose()
        {
        }
    }
}