﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using isdayoff.Contract;
using isdayoff.Core.Exceptions;
using isdayoff.Tests.Extensions;
using NUnit.Framework;

namespace isdayoff.Tests.ErrorMessagesTests
{
    internal class WhenGettingErrorMessage : TestBase
    {
        [Test]
        [TestCaseSource(nameof(ErrorMessagesTestsData))]
        public void ErrorMessagesTranslates(Func<string> getErrorMessage, CultureInfo culture, string pattern)
        {
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            
            var errorMessage = getErrorMessage();

            Assert.That(errorMessage, Does.Match(pattern));
        }

        private static IEnumerable ErrorMessagesTestsData()
        {
            var errorMessagesFunctions = new List<Func<string>>
                               {
                                   ErrorsMessages.UnknownCountry,
                                   ErrorsMessages.UnknownResult,
                                   ErrorsMessages.CacheCanNotBeNull,
                                   ErrorsMessages.SomethingWrongWithTheService,
                                   ErrorsMessages.ExternalServiceDidNotHandleTheRequest,
                                   () => ErrorsMessages.DatesRangeNotSupports(04.08.Of(2020), 04.08.Of(2020), Country.Russia),
                                   () => ErrorsMessages.CanNotFindDayOffInfo(04.08.Of(2020), 04.08.Of(2020), Country.Russia),
                               };

            foreach (var errorMessagesFunction in errorMessagesFunctions)
            {
                yield return new TestCaseData(errorMessagesFunction, new CultureInfo("ru-RU"), "[ЁёА-я]");
                yield return new TestCaseData(errorMessagesFunction, new CultureInfo("en-US"), "[A-Za-z]");
            }
        }
    }
}