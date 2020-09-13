using System;
using System.Diagnostics;
using System.Threading.Tasks;
using NUnit.Framework;

namespace isdayoff.Tests
{
    public class DebugCommand
    {
        [Test]
        public async Task Test()
        {
            var isOff = new IsDayOff();
            var sw = Stopwatch.StartNew();
            var res = await isOff.CheckDayAsync(DateTime.Now);
            sw.Stop();
        }
    }
} 