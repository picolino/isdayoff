using System.Net.Http;

namespace isdayoff.Core.Http
{
    internal interface IHttpClientFactory
    {
        IHttpClient CreateHttpClient();
    }
}