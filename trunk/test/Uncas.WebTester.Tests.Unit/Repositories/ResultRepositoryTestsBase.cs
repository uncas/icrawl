namespace Uncas.WebTester.Repositories.Tests.Unit
{
    using System;
    using NUnit.Framework;
    using Uncas.WebTester.Models;

    public abstract class ResultRepositoryTestsBase
    {
        private IResultRepository repository;

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
            var entry = new HyperLink(new Uri("http://www.google.com"));

            // Act:
            this.repository.Add(entry);
        }

        protected abstract IResultRepository GetRepository();
    }
}