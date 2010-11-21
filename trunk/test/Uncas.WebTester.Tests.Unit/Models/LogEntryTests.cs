namespace Uncas.WebTester.Models.Tests.Unit
{
    using System;
    using NUnit.Framework;

    [TestFixture]
    public class LogEntryTests
    {
        [Test]
        public void LogEntry_NullException_ArgumentNullException()
        {
            Exception exception = null;
            Assert.Throws<ArgumentNullException>(() => new LogEntry(exception));
        }

        [Test]
        public void Version_Default_NotNull()
        {
            Assert.NotNull(LogEntry.Version);
        }
    }
}
