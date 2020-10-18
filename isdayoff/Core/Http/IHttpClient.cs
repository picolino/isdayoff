using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace isdayoff.Core.Http
{
    internal interface IHttpClient : IDisposable
    {
        Task<HttpResponseMessage> GetAsync(string url, CancellationToken cancellationToken);
    }
}