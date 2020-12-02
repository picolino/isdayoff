using NUnit.Framework;

namespace isdayoff.Tests.HttpClientFactoryTests
{
    internal class WhenCreatingHttpClient : HttpClientFactoryTestBase
    {
        [Test]
        public void NewHttpClientReturnsForEachCreationRequest()
        {
            var firstHttpClient = HttpClientFactory.CreateHttpClient();
            var secondHttpClient = HttpClientFactory.CreateHttpClient();

            Assert.That(firstHttpClient, Is.Not.EqualTo(secondHttpClient));
        }
    }
}