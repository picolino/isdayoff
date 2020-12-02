using System.Globalization;
using System.Threading;
using isdayoff.Tests._Fakes;
using NUnit.Framework;

namespace isdayoff.Tests
{
    internal class TestBase
    {
        protected const string ApiBaseUrlStub = "https://dev.isdayoff.ru/api/";
        protected const string UserAgentStub = "isdayoff-dotnet-lib/1.0 (maintainer: picolino)";
        
        protected IsDayOffApiClientStub ApiClientStub { get; private set; }
        protected IsDayOffCacheStub CacheStub { get; private set; }
        protected HttpMessageHandlerMock HttpMessageHandlerMock { get; private set; }
        
        [SetUp]
        public virtual void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            ApiClientStub = new IsDayOffApiClientStub();
            CacheStub = new IsDayOffCacheStub();
            HttpMessageHandlerMock = new HttpMessageHandlerMock();
        }
    }
}