using System;
using System.Collections;
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
            
            var result = await IsDayOffApiClient.GetDataAsync(
                             04.08.Of(2020), 
                             04.08.Of(2020), 
                             Country.Russia, 
                             false,
                             false,
                             false,
                             CancellationToken.None);

            Assert.That(result.Result, Is.EqualTo(response));
        }
        
        [Test]
        [TestCaseSource(nameof(UrlConstructionTestData))]
        public async Task<string> UrlConstructsCorrectly(
            DateTime from, 
            DateTime to, 
            Country country, 
            bool useShortDays,
            bool treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
            bool useSixDaysWorkWeek, 
            string fakeResponse)
        {
            HttpMessageHandlerMock.ResponseMessage = new HttpResponseMessage{Content = new StringContent(fakeResponse)};
            
            await IsDayOffApiClient.GetDataAsync(
                from, 
                to, 
                country, 
                useShortDays,
                treatNonWorkingDaysByCovidAsWorkingDayAdvanced,
                useSixDaysWorkWeek,
                CancellationToken.None);
            
            return HttpMessageHandlerMock.LastRequest.RequestUri.ToString();
        }

        [Test]
        public async Task RequestHasCorrectUserAgentHeader()
        {
            HttpMessageHandlerMock.ResponseMessage = new HttpResponseMessage{Content = new StringContent("0")};
            
            await IsDayOffApiClient.GetDataAsync(
                04.08.Of(2020), 
                04.08.Of(2020), 
                Country.Russia, 
                false,
                false,
                false,
                CancellationToken.None);

            Assert.That(HttpMessageHandlerMock.LastRequest.Headers.UserAgent.ToString(), Is.EqualTo(UserAgentStub));
        }

        private static IEnumerable UrlConstructionTestData()
        {
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, false, false, false, "0")
                { TestName = "One day passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ru&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 05.08.Of(2020), Country.Russia, false, false, false, "00")
                { TestName = "Two days passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200805&cc=ru&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(31.08.Of(2020), 05.09.Of(2020), Country.Russia, false, false, false, "000000")
                { TestName = "Multiple days in different months passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200831&date2=20200905&cc=ru&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, true, false, false, "0")
                { TestName = "Short days setting passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ru&pre=1&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, false, true, false, "0")
                { TestName = "Covid setting passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ru&pre=0&covid=1&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, false, false, true, "0")
                { TestName = "Six days setting passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ru&pre=0&covid=0&sd=1" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Russia, false, false, false, "000000")
                { TestName = "Russia passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ru&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Belarus, false, false, false, "0")
                { TestName = "Belarus passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=by&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Kazakhstan, false, false, false, "0")
                { TestName = "Kazakhstan passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=kz&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Ukraine, false, false, false, "0")
                { TestName = "Ukraine passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=ua&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.USA, false, false, false, "0")
                { TestName = "USA passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=us&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Uzbekistan, false, false, false, "0")
                { TestName = "Uzbekistan passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=uz&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Turkey, false, false, false, "0")
                { TestName = "Turkey passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=tr&pre=0&covid=0&sd=0" };
            yield return new TestCaseData(04.08.Of(2020), 04.08.Of(2020), Country.Latvia, false, false, false, "0")
                { TestName = "Latvia passes correctly", ExpectedResult = $"{ApiBaseUrlStub}getdata?date1=20200804&date2=20200804&cc=lv&pre=0&covid=0&sd=0" };
        }

        [Test]
        public void UnknownPassedCountryThrowsArgumentOutOfRangeException()
        {
            async Task Act()
            {
                await IsDayOffApiClient.GetDataAsync(
                    04.08.Of(2020), 
                    04.08.Of(2020),
                    (Country) int.MaxValue, 
                    false,
                    false,
                    false,
                    CancellationToken.None);
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
                await IsDayOffApiClient.GetDataAsync(
                    04.08.Of(2020), 
                    06.08.Of(2020),
                    Country.Russia, 
                    false,
                    false,
                    false,
                    CancellationToken.None);
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
                             TestName = "IfResponseCodeIsOkButResultLengthDoesNotMatchRequestedDaysThenDaysCountMismatchExceptionThrows",
                             ExpectedResult = typeof(DaysCountMismatchException)
                         };
        }
    }
}