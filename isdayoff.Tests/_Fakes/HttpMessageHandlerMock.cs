using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace isdayoff.Tests._Fakes
{
    public class HttpMessageHandlerMock : HttpMessageHandler
    {
        public HttpRequestMessage LastRequest { get; set; }
        public HttpResponseMessage ResponseMessage { get; set; }
        
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequest = request;
            return Task.FromResult(ResponseMessage);
        }
    }
}