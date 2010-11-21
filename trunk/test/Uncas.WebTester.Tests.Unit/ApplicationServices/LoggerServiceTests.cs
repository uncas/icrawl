namespace Uncas.WebTester.Tests.Unit.ApplicationServices
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Repositories;

    [TestFixture]
    public class LoggerServiceTests
    {
        private ILoggerService loggerService;

        [SetUp]
        public void BeforeEach()
        {
            var logEntryRepository = new Mock<ILogEntryRepository>().Object;
            this.loggerService = new LoggerService(logEntryRepository);
        }

        [TearDown]
        public void AfterEach()
        {
            this.loggerService = null;
        }

        [Test]
        public void LogError_WithException_Ok()
        {
            var exception = new Exception("Test");
            this.loggerService.LogError(exception);
        }

        [Test]
        public void LogError_WithExceptionAndMessage_Ok()
        {
            var exception = new Exception("Test");
            this.loggerService.LogError(exception, "Test");
        }

        [Test]
        public void LogError_WithMessage_Ok()
        {
            this.loggerService.LogError("Test");
        }
    }
}
