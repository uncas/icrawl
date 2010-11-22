namespace Uncas.WebTester.ApplicationServices.Tests.Integration
{
    using System;
    using Moq;
    using NUnit.Framework;
    using Uncas.WebTester.Utilities;

    [TestFixture]
    public class CrawlerServiceTests
    {
        private readonly Uri ExistingUrl = new Uri("http://www.uncas.dk");

        private ICrawlerService service;

        [SetUp]
        public void BeforeEach()
        {
            var browserUtility =
                new HtmlAgilityBrowserUtility();
            var resultServiceMock =
                new Mock<IResultService>();
            this.service =
                new CrawlerService(
                    resultServiceMock.Object,
                    browserUtility);
        }

        [TearDown]
        public void AfterEach()
        {
            this.service = null;
        }

        [Test]
        public void Crawl_Default_Ok()
        {
            // Arrange:
            CrawlConfiguration configuration =
                new CrawlConfiguration(this.ExistingUrl, 10);

            // Act:
            this.service.Crawl(configuration);

            // Assert:
        }
    }
}