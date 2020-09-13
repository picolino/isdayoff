using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core.Cache;

namespace isdayoff.Core
{
    internal class IsDayOffService
    {
        private readonly IsDayOffSettings settings;
        private readonly IIsDayOffApiClient apiClient;
        private readonly IsDayOffCache cache;
        
        public IsDayOffService(IsDayOffSettings settings, IIsDayOffApiClient apiClient)
        {
            this.settings = settings;
            this.apiClient = apiClient;
            cache = new IsDayOffCache();
        }

        public async Task<List<DayOffDateTime>> CheckYearAsync(int year, DayOffCountry country)
        {
            if (settings.UseCache)
            {
                if (cache.TryGetCachedYear(year, country, out var cachedResult))
                {
                    return cachedResult;
                }
            }
            
            var response = await apiClient.GetDataAsync(year, country);
            var daysInYear = CreateDateRangeForYear(year);
            var result = GenerateDayOffDateTimeList(response.Result, daysInYear);

            if (settings.UseCache)
            {
                cache.SaveYearInCache(year, country, result);
            }

            return result;
        }

        public async Task<List<DayOffDateTime>> CheckMonthAsync(int year, int month, DayOffCountry country)
        {
            if (settings.UseCache)
            {
                if (cache.TryGetCachedMonth(year, month, country, out var cachedResult))
                {
                    return cachedResult;
                }
            }
            
            var response = await apiClient.GetDataAsync(year, month, country);
            var daysInMonth = CreateDateRangeForMonth(year, month);
            var result = GenerateDayOffDateTimeList(response.Result, daysInMonth);

            if (settings.UseCache)
            {
                cache.SaveMonthInCache(year, month, country, result);
            }

            return result;
        }
        
        public async Task<DayType> CheckDayAsync(int year, int month, int day, DayOffCountry country)
        {
            if (settings.UseCache)
            {
                if (cache.TryGetCachedDay(year, month, day, country, out var cachedResult))
                {
                    return cachedResult;
                }
            }
            
            var response = await apiClient.GetDataAsync(year, month, day, country);
            var charDayRepresentation = response.Result.Single();
            var result = ConvertCharToDateType(charDayRepresentation);

            if (settings.UseCache)
            {
                cache.SaveDayInCache(year, month, day, country, result);
            }
            
            return result;
        }

        private static List<DayOffDateTime> GenerateDayOffDateTimeList(string responseResult, IReadOnlyList<DateTime> dates)
        {
            var results = new DayOffDateTime[responseResult.Length];
            
            for (var i = 0; i < dates.Count; i++)
            {
                var day = dates[i];
                var charDayRepresentation = responseResult[i];
                var dayType = ConvertCharToDateType(charDayRepresentation);
                results[i] = new DayOffDateTime(day, dayType);
            }

            return results.ToList();
        }

        private static List<DateTime> CreateDateRangeForYear(int year)
        {
            var dates = new List<DateTime>();

            for (var date = new DateTime(year, 1, 1); date.Year == year; date = date.AddDays(1))
            {
                dates.Add(date);       
            }

            return dates;
        }

        private static List<DateTime> CreateDateRangeForMonth(int year, int month)
        {
            var dates = new List<DateTime>();

            for (var date = new DateTime(year, month, 1); date.Month == month; date = date.AddDays(1))
            {
                dates.Add(date);       
            }

            return dates;
        }

        private static DayType ConvertCharToDateType(char character)
        {
            switch (character)
            {
                case '0':
                    return DayType.WorkingDay;
                case '1':
                    return DayType.NotWorkingDay;
                case '2':
                    return DayType.ShortDay;
                case '4':
                    return DayType.WorkingDayAdvanced;
                default:
                    throw new ArgumentOutOfRangeException(nameof(character), character, "Unknown result");
            }
        }
    }
}