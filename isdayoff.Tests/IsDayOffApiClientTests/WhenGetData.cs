using System;
using System.Collections;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Exceptions;
using isdayoff.Tests._Extensions;
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
            HttpMessageHandlerMock.ResponseMessage = new HttpResponseMessage{Content = new StringContent(response)};
            
            var result = await IsDayOffApiClient.GetDataAsync(04.08.Of(2020), 04.08.Of(2020), Country.Russia, CancellationToken.None);

            Assert.That(result.Result, Is.EqualTo(response));
        }
        
        [Test]
        [TestCaseSource(nameof(UrlConstructionTestData))]
        public async Task<string> UrlConstructsCorrectly(DateTime from, DateTime to, Country country, string fakeResponse)
        {
            HttpMessageHandlerMock.ResponseMessage = new HttpResponseMessage{Content = new StringContent(fakeResponse)};
            
            await IsDayOffApiClient.GetDataAsync(from, to, country, CancellationToken.None);
            
            return HttpMessageHandlerMock.LastRequest.RequestUri.ToString();
        }

        [Test]
        public async Task RequestHasCorrectUserAgentHeader()
        {
            HttpMessageHandlerMock.ResponseMessage = new HttpResponseMessage{Content = new StringContent("0")};
            
            await IsDayOffApiClient.GetDataAsync(04.08.Of(2020), 04.08.Of(2020), Country.Russia, CancellationToken.None);

            Assert.That(HttpMessageHandlerMock.LastRequest.Headers.UserAgent.ToString(), Is.EqualTo(UserAgentStub));
        }

        private static IEnumerable UrlConstructionTestData()
        {
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, "0")
                         {TestName = "One day passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ru"};
            yield return new TestCaseData(04.08.Of(2020), 05.08.Of(2020), Country.Russia, "00")
                         {TestName = "Two days passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200805&cc=ru"};
            yield return new TestCaseData(31.08.Of(2020), 05.09.Of(2020), Country.Russia, "000000")
                         {TestName = "Multiple days in different months passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200831&date2=20200905&cc=ru"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, "0")
                         {TestName = "Russia passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ru"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Belarus, "0")
                         {TestName = "Belarus passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=by"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Kazakhstan, "0")
                         {TestName = "Kazakhstan passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=kz"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Ukraine, "0")
                         {TestName = "Ukraine passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ua"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.USA, "0")
                         {TestName = "USA passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=us"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Uzbekistan, "0")
                         {TestName = "Uzbekistan passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=uz"};
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Turkey, "0")
                         {TestName = "Turkey passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=tr"};
        }

        [Test]
        public void UnknownPassedCountryThrowsArgumentOutOfRangeException()
        {
            async Task Act()
            {
                await IsDayOffApiClient.GetDataAsync(04.08.Of(2020), 04.08.Of(2020), (Country) int.MaxValue, CancellationToken.None);
            }

            Assert.ThrowsAsync<ArgumentOutOfRangeException>(Act);
        }

        [Test]
        [TestCaseSource(nameof(InvalidTestData))]
        public Type ValidationWorksCorrectly(string responseStringContent,
                                             HttpStatusCode responseStatusCode)
        {
            HttpMessageHandlerMock.ResponseMessage = new HttpResponseMessage(responseStatusCode){Content = new StringContent(responseStringContent)};

            async Task Act()
            {
                await IsDayOffApiClient.GetDataAsync(04.08.Of(2020), 06.08.Of(2020), Country.Russia, CancellationToken.None);
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
            yield return new TestCaseData("4", HttpStatusCode.OK)
                         {
                             TestName = "IfResponseCodeIsOkButResultLenghtDoesNotMatchRequestedDaysThenDaysCountMismatchExceptionThrows",
                             ExpectedResult = typeof(DaysCountMismatchException)
                         };
        }
    }
}