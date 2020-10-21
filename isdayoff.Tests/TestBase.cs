using System.Globalization;
using System.Threading;
using isdayoff.Tests.Fakes;
using NUnit.Framework;

namespace isdayoff.Tests
{
    internal class TestBase
    {
        protected IsDayOffApiClientStub ApiClientStub { get; private set; }
        protected IsDayOffCacheStub CacheStub { get; private set; }
        protected HttpClientStubFactory HttpClientStubFactory { get; private set; }
        protected HttpClientStub HttpClientStub { get; private set; }
        
        [SetUp]
        public virtual void Setup()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;

            ApiClientStub = new IsDayOffApiClientStub();
            CacheStub = new IsDayOffCacheStub();
            HttpClientStub = new HttpClientStub();
            HttpClientStubFactory = new HttpClientStubFactory(HttpClientStub);
        }
    }
}