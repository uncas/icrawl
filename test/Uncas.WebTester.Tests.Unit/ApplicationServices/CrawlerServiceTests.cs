namespace Uncas.WebTester.ApplicationServices.Tests.Unit
{
    using System;
    using System.Net;
    using Moq;
    using NUnit.Framework;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Tests.Unit.Mocks;
    using Uncas.WebTester.Utilities;

    [TestFixture]
    public class CrawlerServiceTests
    {
        private readonly Uri ExistingUrl = new Uri("http://www.google.dk");

        private Mock<IBrowserUtility> browserUtilityMock;

        private ResultServiceMock resultService;

        private ICrawlerService crawlerService;

        [SetUp]
        public void BeforeEach()
        {
            this.browserUtilityMock =
                new Mock<IBrowserUtility>();
            this.resultService =
                new ResultServiceMock();
            this.crawlerService =
                new CrawlerService(
                    this.resultService,
                    this.browserUtilityMock.Object);
        }

        [TearDown]
        public void AfterEach()
        {
            this.resultService = null;
            this.crawlerService = null;
        }

        [Test]
        public void Crawl_NullNavigateResult_Ok()
        {
            // Arrange:
            CrawlConfiguration configuration =
                GetCrawlConfiguration(this.ExistingUrl);

            // Act:
            this.crawlerService.Crawl(configuration);

            // Assert:
        }

        [Test]
        public void Crawl_Default_Ok()
        {
            // Arrange:
            CrawlConfiguration configuration =
                GetCrawlConfiguration(this.ExistingUrl);
            var navigateResult =
                GetNavigateResult(HttpStatusCode.OK);
            this.browserUtilityMock.Setup(
                b => b.NavigateTo(It.IsAny<Uri>()))
                .Returns(navigateResult);

            // Act:
            this.crawlerService.Crawl(configuration);

            // Assert:
        }

        [Test]
        public void Crawl_NoLinks_OnlyTheOriginalResult()
        {
            // Arrange:
            CrawlConfiguration configuration =
                GetCrawlConfiguration(this.ExistingUrl);
            var navigateResult =
                GetNavigateResult(HttpStatusCode.OK);
            this.browserUtilityMock.Setup(
                b => b.NavigateTo(It.IsAny<Uri>()))
                .Returns(navigateResult);

            // Act:
            this.crawlerService.Crawl(configuration);

            // Assert:
            Assert.That(this.resultService.NumberOfResults, Is.EqualTo(1));
        }

        [Test]
        public void Crawl_BrowseError_NullHandled()
        {
            // Arrange:
            CrawlConfiguration configuration =
                GetCrawlConfiguration(this.ExistingUrl);
            var navigateResult =
                GetNavigateResult(HttpStatusCode.OK);
            this.browserUtilityMock.Setup(
                b => b.NavigateTo(It.IsAny<Uri>()))
                .Throws(new Exception());

            // Act:
            this.crawlerService.Crawl(configuration);

            // Assert:
            Assert.That(this.resultService.NumberOfResults, Is.EqualTo(1));
        }

        [Test]
        public void Crawl_OneLinkToPageWithoutLinks_TwoResults()
        {
            // Arrange:
            var startUrl = new Uri("http://fake.somewhere.net");
            CrawlConfiguration configuration =
                GetCrawlConfiguration(startUrl);
            var otherUrl = new Uri("http://fake.somewhere.net/about");
            var startNavigateResult =
                GetNavigateResult(HttpStatusCode.OK);
            startNavigateResult.AddLink(new HyperLink(otherUrl));
            var otherNavigateResult =
                GetNavigateResult(HttpStatusCode.OK);
            this.browserUtilityMock.Setup(
                b => b.NavigateTo(startUrl))
                .Returns(startNavigateResult);
            this.browserUtilityMock.Setup(
                b => b.NavigateTo(otherUrl))
                .Returns(otherNavigateResult);

            // Act:
            this.crawlerService.Crawl(configuration);

            // Assert:
            Assert.That(this.resultService.NumberOfResults, Is.EqualTo(2));
        }

        /// <summary>
        /// Gets the crawl configuration.
        /// </summary>
        /// <param name="url">The start URL.</param>
        /// <returns>The crawl configuration.</returns>
        private static CrawlConfiguration GetCrawlConfiguration(
            Uri url)
        {
            return new CrawlConfiguration(
                url,
                10);
        }

        private static NavigateResult GetNavigateResult(
           HttpStatusCode statusCode)
        {
            return new NavigateResult(statusCode, 2, 3, 2);
        }
    }
}