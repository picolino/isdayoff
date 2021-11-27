using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using isdayoff.Contract;
using isdayoff.Core;
using isdayoff.Core.Exceptions;
using isdayoff.Core.Extensions;
using isdayoff.Core.Http;

namespace isdayoff
{
    /// <summary>
    /// Basic class for operating with isdayoff API
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IsDayOff
    {
        internal static readonly TraceSource Tracer = new TraceSource(nameof(IsDayOff));
        
        private readonly IsDayOffSettings settings;
        private readonly IsDayOffService service;
        
        /// <summary>
        /// Makes IsDayOff with default settings.
        /// Default country sets to Russia and no in-memory cache use.
        /// </summary>
        public IsDayOff() : this(IsDayOffSettings.Default)
        {
        }

        /// <summary>
        /// Makes IsDayOff with overriden settings.
        /// </summary>
        /// <param name="settings">Settings</param>
        /// <exception cref="ArgumentNullException">Thrown when some not null property is set to null</exception>
        public IsDayOff(IsDayOffSettings settings)
            : this(
                settings,
                new IsDayOffApiClient(
                    settings.ApiBaseUrl,
                    settings.UserAgent,
                    new HttpClientFactory(new HttpClientHandler())))
        {
        }

        internal IsDayOff(IsDayOffSettings settings, IIsDayOffApiClient apiClient)
        {
            this.settings = settings ?? throw new ArgumentNullException(nameof(settings), ErrorsMessages.SettingCanNotBeNull());
            
            if (settings.TraceLevel.HasValue)
            {
                Tracer.Switch.Level = settings.TraceLevel.Value;
            }
            
            service = new IsDayOffService(apiClient, settings.Cache);
        }

        /// <summary>
        /// Get dates with day off information for year of default country
        /// </summary>
        /// <param name="year">Year to get dates with day off information</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckYearAsync(int year)
        {
            return await CheckYearAsync(
                       year, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get dates with day off information for year of default country
        /// </summary>
        /// <param name="year">Year to get dates with day off information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckYearAsync(
            int year, 
            CancellationToken cancellationToken)
        {
            return await CheckYearAsync(
                       year, 
                       settings.DefaultCountry, 
                       cancellationToken);
        }

        /// <summary>
        /// Get dates with day off information for year
        /// </summary>
        /// <param name="year">Year to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckYearAsync(
            int year, 
            Country country)
        {
            return await CheckYearAsync(
                       year, 
                       country, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get dates with day off information for year
        /// </summary>
        /// <param name="year">Year to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckYearAsync(
            int year, 
            Country country, 
            CancellationToken cancellationToken)
        {
            var yearDateTime = new DateTime(year, 1, 1);
            
            return await CheckDatesRangeAsync(
                       yearDateTime, 
                       yearDateTime.EndOfYear(), 
                       country, 
                       cancellationToken);
        }
        
        /// <summary>
        /// Get dates with day off information for month of specific year of default country
        /// </summary>
        /// <param name="year">Year of month</param>
        /// <param name="month">Month to get dates with day off information</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckMonthAsync(
            int year, 
            int month)
        {
            return await CheckMonthAsync(
                       year, 
                       month, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get dates with day off information for month of specific year of default country
        /// </summary>
        /// <param name="year">Year of month</param>
        /// <param name="month">Month to get dates with day off information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckMonthAsync(
            int year, 
            int month, 
            CancellationToken cancellationToken)
        {
            return await CheckMonthAsync(
                       year, 
                       month, 
                       settings.DefaultCountry, 
                       cancellationToken);
        }

        /// <summary>
        /// Get dates with day off information for month of specific year
        /// </summary>
        /// <param name="year">Year of month</param>
        /// <param name="month">Month to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckMonthAsync(
            int year, 
            int month, 
            Country country)
        {
            return await CheckMonthAsync(
                       year, 
                       month, 
                       country, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get dates with day off information for month of specific year
        /// </summary>
        /// <param name="year">Year of month</param>
        /// <param name="month">Month to get dates with day off information</param>
        /// <param name="country">Country to get dates with day off information for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>List of dates with day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckMonthAsync(
            int year, 
            int month, 
            Country country, 
            CancellationToken cancellationToken)
        {
            var monthDateTime = new DateTime(year, month, 1);
            
            return await CheckDatesRangeAsync(
                       monthDateTime, 
                       monthDateTime.EndOfMonth(), 
                       country, 
                       cancellationToken);
        }
        
        /// <summary>
        /// Get day off information for day of specific month of specific year of default country
        /// </summary>
        /// <param name="year">Year of day</param>
        /// <param name="month">Month of day</param>
        /// <param name="day">Day to get day off information</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(
            int year, 
            int month, 
            int day)
        {
            return await CheckDayAsync(
                       year, 
                       month, 
                       day, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get day off information for day of specific month of specific year of default country
        /// </summary>
        /// <param name="year">Year of day</param>
        /// <param name="month">Month of day</param>
        /// <param name="day">Day to get day off information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(
            int year, 
            int month, 
            int day, 
            CancellationToken cancellationToken)
        {
            return await CheckDayAsync(
                       year, 
                       month, 
                       day, 
                       settings.DefaultCountry, 
                       cancellationToken);
        }
        
        /// <summary>
        /// Get day off information for day of specific month of specific year of default country
        /// </summary>
        /// <param name="day">DateTime of specific day to get day off information</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(DateTime day)
        {
            return await CheckDayAsync(day, CancellationToken.None);
        }

        /// <summary>
        /// Get day off information for day of specific month of specific year of default country
        /// </summary>
        /// <param name="day">DateTime of specific day to get day off information</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(
            DateTime day, 
            CancellationToken cancellationToken)
        {
            return await CheckDayAsync(
                       day, 
                       settings.DefaultCountry, 
                       cancellationToken);
        }
        
        /// <summary>
        /// Get day off information for day of specific month of specific year
        /// </summary>
        /// <param name="day">DateTime of specific day to get day off information</param>
        /// <param name="country">Country to get day off information for</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(
            DateTime day, 
            Country country)
        {
            return await CheckDayAsync(
                       day, 
                       country, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get day off information for day of specific month of specific year
        /// </summary>
        /// <param name="day">DateTime of specific day to get day off information</param>
        /// <param name="country">Country to get day off information for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(
            DateTime day, 
            Country country, 
            CancellationToken cancellationToken)
        {
            return await CheckDayAsync(
                       day.Year, 
                       day.Month, 
                       day.Day, 
                       country, 
                       cancellationToken);
        }

        /// <summary>
        /// Get day off information for day of specific month of specific year
        /// </summary>
        /// <param name="year">Year of day</param>
        /// <param name="month">Month of day</param>
        /// <param name="day">Day to get day off information</param>
        /// <param name="country">Country to get day off information for</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(
            int year, 
            int month, 
            int day, 
            Country country)
        {
            return await CheckDayAsync(
                       year, 
                       month, 
                       day, 
                       country, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get day off information for day of specific month of specific year
        /// </summary>
        /// <param name="year">Year of day</param>
        /// <param name="month">Month of day</param>
        /// <param name="day">Day to get day off information</param>
        /// <param name="country">Country to get day off information for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<DayType> CheckDayAsync(
            int year, 
            int month, 
            int day, 
            Country country, 
            CancellationToken cancellationToken)
        {
            var dayDateTime = new DateTime(year, month, day);
            
            var result = await CheckDatesRangeAsync(
                             dayDateTime, 
                             dayDateTime, 
                             country, 
                             cancellationToken);
            return result.Single().DayType;
        }

        /// <summary>
        /// Get day off information for dates range
        /// </summary>
        /// <param name="from">Date from (inclusive)</param>
        /// <param name="to">Date to (inclusive)</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckDatesRangeAsync(
            DateTime from, 
            DateTime to)
        {
            return await CheckDatesRangeAsync(
                       from, 
                       to, 
                       CancellationToken.None);
        }

        /// <summary>
        /// Get day off information for dates range
        /// </summary>
        /// <param name="from">Date from (inclusive)</param>
        /// <param name="to">Date to (inclusive)</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckDatesRangeAsync(
            DateTime from, 
            DateTime to, 
            CancellationToken cancellationToken)
        {
            return await CheckDatesRangeAsync(
                       from, 
                       to, 
                       settings.DefaultCountry, 
                       cancellationToken);
        }
        
        /// <summary>
        /// Get day off information for dates range
        /// </summary>
        /// <param name="from">Date from (inclusive)</param>
        /// <param name="to">Date to (inclusive)</param>
        /// <param name="country">Country to get day off information for</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckDatesRangeAsync(
            DateTime from, 
            DateTime to, 
            Country country)
        {
            return await CheckDatesRangeAsync(
                       from, 
                       to, 
                       country,
                       CancellationToken.None);
        }

        /// <summary>
        /// Get day off information for dates range
        /// </summary>
        /// <param name="from">Date from (inclusive)</param>
        /// <param name="to">Date to (inclusive)</param>
        /// <param name="country">Country to get day off information for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckDatesRangeAsync(
            DateTime from, 
            DateTime to, 
            Country country,
            CancellationToken cancellationToken)
        {
            return await CheckDatesRangeAsync(
                       from, 
                       to, 
                       country, 
                       settings.UseShortDays,
                       settings.TreatNonWorkingDaysByCovidAsWorkingDayAdvanced,
                       settings.UseSixDaysWorkWeek,
                       cancellationToken);
        }

        /// <summary>
        /// Get day off information for dates range
        /// </summary>
        /// <param name="from">Date from (inclusive)</param>
        /// <param name="to">Date to (inclusive)</param>
        /// <param name="country">Country to get day off information for</param>
        /// <param name="useSixDaysWorkWeek">Use six days work week</param>
        /// <param name="useShortDays">Use short days</param>
        /// <param name="treatNonWorkingDaysByCovidAsWorkingDayAdvanced">Use working days advanced</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckDatesRangeAsync(
            DateTime from, 
            DateTime to, 
            Country country,
            bool useShortDays,
            bool treatNonWorkingDaysByCovidAsWorkingDayAdvanced, 
            bool useSixDaysWorkWeek, 
            CancellationToken cancellationToken)
        {
            return await CheckAsync(
                       new IsDayOffGetDatesRangeArgs(
                           from,
                           to,
                           country,
                           useShortDays,
                           treatNonWorkingDaysByCovidAsWorkingDayAdvanced,
                           useSixDaysWorkWeek),
                       cancellationToken);
        }

        /// <summary>
        /// Get day off information by parameters
        /// </summary>
        /// <param name="args">Parameters to get day off information for</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckAsync(IsDayOffGetDatesRangeArgs args)
        {
            return await CheckAsync(args, CancellationToken.None);
        }
        
        /// <summary>
        /// Get day off information by parameters
        /// </summary>
        /// <param name="args">Parameters to get day off information for</param>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <returns>Day off information</returns>
        /// <exception cref="IsDayOffExternalServiceException">Throws if error occured while processing request to isdayoff external service</exception>
        public async Task<List<DayOffDateTime>> CheckAsync(
            IsDayOffGetDatesRangeArgs args,
            CancellationToken cancellationToken)
        {
            return await service.CheckDatesRangeAsync(args, cancellationToken);
        }
    }
}