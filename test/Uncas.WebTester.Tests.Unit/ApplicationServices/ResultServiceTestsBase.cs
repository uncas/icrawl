namespace Uncas.WebTester.Tests.Unit.ApplicationServices
{
    using System;
    using NUnit.Framework;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Models;

    [TestFixture]
    public abstract class ResultServiceTestsBase
    {
        private IResultService resultService;

        [SetUp]
        public void BeforeEach()
        {
            this.resultService = this.GetResultService();
        }

        [TearDown]
        public void AfterEach()
        {
            this.resultService = null;
        }

        [Test]
        public void ProcessResult_SimpleLink_Ok()
        {
            var link = new HyperLink(new Uri("http://www.google.dk"));
            this.resultService.ProcessResult(link);
        }

        protected abstract IResultService GetResultService();
    }
}