namespace Uncas.WebTester.Repositories.Tests.Unit
{
    using System;
    using NUnit.Framework;
    using Uncas.WebTester.Models;

    public abstract class LogEntryRepositoryTestsBase
    {
        private ILogEntryRepository repository;

        [SetUp]
        public void BeforeEach()
        {
            this.repository = this.GetRepository();
        }

        [TearDown]
        public void AfterEach()
        {
            this.repository = null;
        }

        [Test]
        public void Add_EntryWithMessage_Ok()
        {
            // Arrange:
            var entry = new LogEntry("test");

            // Act:
            this.repository.Add(entry);
        }

        [Test]
        public void Add_EntryWithException_Ok()
        {
            // Arrange:
            var entry = new LogEntry(this.GetBadMethodException());

            // Act:
            this.repository.Add(entry);
        }

        [Test]
        public void Add_EntryWithExceptionAndMessage_Ok()
        {
            // Arrange:
            var entry = new LogEntry(this.GetBadMethodException(), "test");

            // Act:
            this.repository.Add(entry);
        }

        protected abstract ILogEntryRepository GetRepository();

        private Exception GetBadMethodException()
        {
            try
            {
                throw new Exception("bad bad method");
            }
            catch (Exception ex)
            {
                return ex;
            }
        }
    }
}