using NUnit.Framework;

namespace isdayoff.Tests.IsDayOffSettingsTests
{
    internal class WhenBuild : IsDayOffSettingsTestBase
    {
        [Test]
        public void BuildAlwaysReturnsUniqueSettingsBuilder()
        {
            var builderFirst = IsDayOffSettings.Build;
            var builderSecond = IsDayOffSettings.Build;

            Assert.That(builderFirst, Is.Not.EqualTo(builderSecond));
        }
    }
}