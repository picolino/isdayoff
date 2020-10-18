using System;
using System.Globalization;
using isdayoff.Contract;

namespace isdayoff.Core.Exceptions
{
    internal static class ErrorsMessages
    {
        private static bool IsRussian => CultureInfo.CurrentCulture.LCID == 1049;
        
        public static string CacheCanNotBeNull()
        {
            return IsRussian
                       ? "Реализация кэша не может быть null. Кэш выключен по-умолчанию, так что не обязательно явно задавать его значение в null"
                       : "Cache implementation can't be null. Cache is disabled by-default so you dont need to set it to null";
        }

        public static string CanNotFindDayOffInfo(DateTime from, DateTime to, Country country)
        {
            return IsRussian
                       ? $"Не найдена информация в диапазоне '{from.ToShortDateString()}-{to.ToShortDateString()}' для страны '{country}'"
                       : $"Cannot find day off information in dates range '{from.ToShortDateString()}-{to.ToShortDateString()}' for country '{country}'";
        }

        public static string DatesRangeNotSupports(DateTime from, DateTime to, Country country)
        {
            return IsRussian
                       ? $"Диапазон дат '{from.ToShortDateString()}-{to.ToShortDateString()}' для страны '{country}' не поддерживается"
                       : $"Dates range '{from.ToShortDateString()}-{to.ToShortDateString()}' for country '{country}' not supports";
        }

        public static string SomethingWrongWithTheService()
        {
            return IsRussian
                       ? "Что-то пошло не так на стороне сервиса"
                       : "Something wrong with service";
        }

        public static string ExternalServiceDidNotHandleTheRequest()
        {
            return IsRussian
                       ? "Внешнему сервису не удалось обработать запрос. Подробности можно посмотреть во внутреннем исключении"
                       : "External service did not handle the request. See details in inner exception";
        }

        public static string UnknownCountry()
        {
            return IsRussian
                       ? "Неизвестная страна"
                       : "Unknown country";
        }

        public static string UnknownResult()
        {
            return IsRussian
                       ? "Неизвестный результат"
                       : "Unknown result";
        }
    }
}