namespace Uncas.WebTester.Utilities.Tests.Unit
{
    using System;
    using System.Net;
    using NUnit.Framework;

    [TestFixture]
    public abstract class BrowserUtilityTestsBase
    {
        protected readonly Uri NonExistingServer =
            new Uri("http://lqkweoi.uncasxxx.dkxx");

        private IBrowserUtility utility;

        protected Uri ExistingUrl
        {
            get
            {
                return this.GetUrl("Test/ValidPage");
            }
        }

        protected Uri NonExistingUrl
        {
            get
            {
                return this.GetUrl("Test/NotFound");
            }
        }

        protected Uri Error500Url
        {
            get
            {
                return this.GetUrl("Test/Error500");
            }
        }

        protected Uri PageWithEmptyLink
        {
            get
            {
                return this.GetUrl("Test/EmptyLink");
            }
        }

        protected virtual int WebsitePort
        {
            get
            {
                return 52349;
            }
        }

        [SetUp]
        public void BeforeEach()
        {
            this.utility = this.GetBrowserUtility();
        }

        [TearDown]
        public void AfterEach()
        {
            this.utility = null;
        }

        [Test]
        public void NavigateTo_Google_Ok()
        {
            // Arrange:
            var url = this.ExistingUrl;

            // Act:
            NavigateResult result = this.utility.NavigateTo(url);

            // Assert:
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void NavigateTo_NonExisting_NotFound()
        {
            // Arrange:
            var url = this.NonExistingUrl;

            // Act:
            NavigateResult result = this.utility.NavigateTo(url);

            // Assert:
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.NotFound));
        }

        [Test]
        public void NavigateTo_Error500Page_GetsError500()
        {
            // Arrange:
            var url = this.Error500Url;

            // Act:
            NavigateResult result = this.utility.NavigateTo(url);

            // Assert:
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.InternalServerError));
        }

        [Test]
        public void NavigateTo_PageWithEmptyLink_HandledOk()
        {
            // Arrange:
            var url = this.PageWithEmptyLink;

            // Act:
            NavigateResult result = this.utility.NavigateTo(url);

            // Assert:
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        }

        [Test]
        public void NavigateTo_NonExistingServer_HandledOk()
        {
            // Arrange:
            var url = this.NonExistingServer;

            // Act:
            NavigateResult result = this.utility.NavigateTo(url);
        }

        protected abstract IBrowserUtility GetBrowserUtility();

        private Uri GetUrl(string path)
        {
            string uriString = string.Format(
                "http://localhost:{0}/{1}",
                this.WebsitePort,
                path);
            return new Uri(uriString);
        }
    }
}