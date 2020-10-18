using System;
using System.Collections;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Exceptions;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffApiClientTests
{
    internal class WhenGetData : IsDayOffApiClientTestBase
    {
        [Test]
        [TestCase("010201000")]
        [TestCase("999999999")]
        [TestCase("response?!")]
        [TestCase("0")]
        public async Task ResponsePassesUnchanged(string response)
        {
            HttpClientStub.ResponseStringContent = response;
            
            var result = await IsDayOffApiClient.GetDataAsync(04.08.Of(2020), 04.08.Of(2020), Country.Russia, CancellationToken.None);

            Assert.That(result.Result, Is.EqualTo(response));
        }
        
        [Test]
        [TestCaseSource(nameof(UrlConstructionTestData))]
        public async Task<string> UrlConstructsCorrectly(DateTime from, DateTime to, Country country, string fakeResponse)
        {
            HttpClientStub.ResponseStringContent = fakeResponse;
            
            await IsDayOffApiClient.GetDataAsync(from, to, country, CancellationToken.None);
            
            return HttpClientStub.LatestGetRequestUrl;
        }

        private static IEnumerable UrlConstructionTestData()
        {
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, "0")
                         {TestName = "One day passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200804&date2=20200804&cc=ru"};
            yield return new TestCaseData(04.08.Of(2020), 05.08.Of(2020), Country.Russia, "00")
                         {TestName = "Two days passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200804&date2=20200805&cc=ru"};
            yield return new TestCaseData(31.08.Of(2020), 05.09.Of(2020), Country.Russia, "000000")
                         {TestName = "Multiple days in different months passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200831&date2=20200905&cc=ru"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, "0")
                         {TestName = "Russia passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200804&date2=20200804&cc=ru"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Belarus, "0")
                         {TestName = "Belarus passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200804&date2=20200804&cc=by"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Kazakhstan, "0")
                         {TestName = "Kazakhstan passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200804&date2=20200804&cc=kz"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Ukraine, "0")
                         {TestName = "Ukraine passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200804&date2=20200804&cc=ua"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.USA, "0")
                         {TestName = "USA passes correctly", ExpectedResult = $"{ApiBaseUrl}getdata?date1=20200804&date2=20200804&cc=us"};
        }

        [Test]
        [TestCaseSource(nameof(InvalidTestData))]
        public Type ValidationWorksCorrectly(string responseStringContent,
                                             HttpStatusCode responseStatusCode)
        {
            HttpClientStub.ResponseStringContent = responseStringContent;
            HttpClientStub.ResponseStatusCode = responseStatusCode;

            async Task Act()
            {
                await IsDayOffApiClient.GetDataAsync(04.08.Of(2020), 04.08.Of(2020), Country.Russia, CancellationToken.None);
            }

            var outerException = Assert.ThrowsAsync<IsDayOffExternalServiceException>(Act);
            return outerException.InnerException!.GetType();
        }

        private static IEnumerable InvalidTestData()
        {
            yield return new TestCaseData("100", HttpStatusCode.BadRequest)
                         {
                             TestName = "IfResponseCodeIsBadRequestAndResponseContentIs100ThenBadDatesRangeExceptionThrows",
                             ExpectedResult = typeof(BadDatesRangeException)
                         };
            yield return new TestCaseData("199", HttpStatusCode.BadRequest)
                         {
                             TestName = "IfResponseCodeIsBadRequestAndResponseContentIs199ThenServiceErrorExceptionThrows",
                             ExpectedResult = typeof(ServiceErrorException)
                         };
            yield return new TestCaseData("101", HttpStatusCode.NotFound)
                         {
                             TestName = "IfResponseCodeIsNotFoundAndResponseContentIs101ThenServiceErrorExceptionThrows",
                             ExpectedResult = typeof(DayOffDataNotFoundException)
                         };
            yield return new TestCaseData(string.Empty, HttpStatusCode.InternalServerError)
                         {
                             TestName = "IfResponseCodeIsInternalServerErrorThenHttpRequestExceptionThrows",
                             ExpectedResult = typeof(HttpRequestException)
                         };
        }
    }
}