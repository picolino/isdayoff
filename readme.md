<img src="isdayoff.ico" alt="logo" align="right"/>

# IsDayOff

[![Check-in](https://github.com/picolino/isdayoff/workflows/Check-in/badge.svg)](https://github.com/picolino/isdayoff)
[![Nuget](https://img.shields.io/nuget/v/isdayoff)](https://www.nuget.org/packages/isdayoff/)
[![GitHub](https://img.shields.io/github/license/picolino/isdayoff?color=blue)](https://github.com/picolino/isdayoff/blob/master/license)
[![CodeFactor](https://www.codefactor.io/repository/github/picolino/isdayoff/badge)](https://www.codefactor.io/repository/github/picolino/isdayoff)
[![Coverage Status](https://coveralls.io/repos/github/picolino/isdayoff/badge.svg)](https://coveralls.io/github/picolino/isdayoff)

IsDayOff is a .NET library for API isdayoff service (https://isdayoff.ru/)

## Target Frameworks
[![DotNetStandard20](https://img.shields.io/badge/.NET%20Standard-2.0-informational.svg)](https://docs.microsoft.com/ru-ru/dotnet/standard/net-standard)

## Quick start

To work with IsDayOff API you need only create `IsDayOff` class instance and call its methods.
IsDayOff for .NET also provides useful configuration `IsDayOffSettings` 
for configure behaviour you need.

```c#
var defaultSettings = IsDayOffSettings.Default;
var settings = IsDayOffSettings.Build
                               .UseDefaultCountry(Country.USA)
                               .Create();

var isDayOff = new IsDayOff(settings);

var today = DateTime.Today;

DayType todayDayOffInfo = await isDayOff.CheckDayAsync(today);
List<DayOffDateTime> currentMonthDayOffInfo = await isDayOff.CheckMonthAsync(today.Year, today.Month);
List<DayOffDateTime> currentYearDayOffInfo = await isDayOff.CheckYearAsync(today.Year);

List<DayOffDateTime> currentYearDayOffInfoForRussia = await isDayOff.CheckYearAsync(today.Year, Country.Russia);
```

## Default In-Memory Cache
IsDayOff for .NET provides built-in in-memory cache (disabled by-default)

```c#
var settings = IsDayOffSettings.Build
                               .UseInMemoryCache() // Enable cache
                               .Create();
var isDayOff = new IsDayOff(settings);
var firstRequest = await isDayOff.CheckDayAsync(DateTime.Today); // Performs request to external service
var secondRequest = await isDayOff.CheckDayAsync(DateTime.Today); // No request performs
```

It works also if you trying to get day off information for inner date range:
```c#
var firstRequestForYear = await isDayOff.CheckYearAsync(DateTime.Today.Year); // Performs request to external service for current year
var secondRequestForMonthWithinYear = await isDayOff.CheckMonthAsync(DateTime.Today.Year, 06); // No real request performs because year cache by previous request used
```

### Custom cache implementation
You can also inject your custom cache realization through 
implementation of `IIsDayOfCache` interface and register it in settings:

```c#
var customCache = new CustomCache(); // CustomCache must implement IIsDayOfCache
var settings = IsDayOffSettings.Build
                               .UseCustomCache(customCache) // Inject cache
                               .Create();
var isDayOff = new IsDayOff(settings);
```

It is useful if you want to cache external service responses in file or in database.

## FAQ
**Q: Is this library fully thread safe?**  
A: Yes. You can use one instance between multiple threads with no doubt.

**Q: Why some resources (urls) are not available through that library?**  
A: Because that functions built into .NET library or they can be created using already existing methods of library.

Here a full list of isdayoff resources that is not implemented in library and analogues that can be used to achieve similar behaviour:

| isdayoff resource       | .NET analogue                                             |
|-------------------------|-----------------------------------------------------------|
| `/now`                  | `DateTime.Now`                                            |
| `/today`                | `new IsDayOff().CheckDayAsync(DateTime.Today)`            |
| `/tomorrow`             | `new IsDayOff().CheckDayAsync(DateTime.Today.AddDays(1))` |
| `/api/isleap?year=YYYY` | `DateTime.IsLeapYear(YYYY)`                               |

## License

[MIT](https://github.com/picolino/isdayoff/blob/master/license)
