namespace Uncas.WebTester.ApplicationServices.Tests.Unit
{
    using System;
    using System.Net;
    using Moq;
    using NUnit.Framework;
    using Uncas.WebTester.Repositories;
    using Uncas.WebTester.Tests.Unit.Mocks;
    using Uncas.WebTester.Tests.Unit.Stubs;
    using Uncas.WebTester.Utilities;

    [TestFixture]
    public class UrlListCheckerServiceTests
    {
        private IUrlListCheckerService service;

        private Mock<IBrowserUtility> browserUtilityMock;

        private ResultServiceMock resultService;

        [SetUp]
        public void BeforeEach()
        {
            this.browserUtilityMock =
                new Mock<IBrowserUtility>();
            var navigateResult =
                GetNavigateResult(HttpStatusCode.OK);
            this.browserUtilityMock.Setup(
                b => b.NavigateTo(It.IsAny<Uri>()))
                .Returns(navigateResult);
            this.resultService =
                new ResultServiceMock();
            this.service = new UrlListCheckerService(
                this.browserUtilityMock.Object,
                this.resultService);
        }

        [TearDown]
        public void AfterEach()
        {
            this.service = null;
        }

        [Test]
        public void CheckUrlList_NoLinks_ZeroResults()
        {
            // Arrange:
            var providerStub = new Mock<IUrlListProvider>();
            var provider = providerStub.Object;

            // Act:
            this.service.CheckUrlList(provider);

            // Assert:
            Assert.That(this.resultService.NumberOfResults, Is.EqualTo(0));
        }

        [Test]
        public void CheckUrlList_SevenLink_SevenResults()
        {
            // Arrange:
            const int NumberOfLinks = 7;
            var provider = new StubUrlListProvider(NumberOfLinks);

            // Act:
            this.service.CheckUrlList(provider);

            // Assert:
            Assert.That(
                this.resultService.NumberOfResults,
                Is.EqualTo(NumberOfLinks));
        }

        private static NavigateResult GetNavigateResult(
           HttpStatusCode statusCode)
        {
            return new NavigateResult(statusCode, 2, 3, 2);
        }
    }
}