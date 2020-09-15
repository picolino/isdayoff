﻿<img src="isdayoff.ico" alt="logo" align="right"/>

# IsDayOff

[![Check-in](https://github.com/picolino/isdayoff/workflows/Check-in/badge.svg)](https://github.com/picolino/isdayoff)
[![Nuget](https://img.shields.io/nuget/v/isdayoff)](https://www.nuget.org/packages/isdayoff/)

IsDayOff is a .NET library for API isdayoff service (https://isdayoff.ru/)

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
var firstRequest = isDayOff.CheckDayAsync(DateTime.Today); // Performs request to external service
var secondRequest = isDayOff.CheckDayAsync(DateTime.Today); // No request performs
```

_* Note that cache working per-method only. This means if you get day 
off information for specific year (using `CheckYearAsync` method) 
and next request trying to get information for any month (`CheckMonthAsync`) 
or day (`CheckDayAsync`) of this year, additional request will be performed.
However, it is likely this behavior will change in future._

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

## License
MIT
