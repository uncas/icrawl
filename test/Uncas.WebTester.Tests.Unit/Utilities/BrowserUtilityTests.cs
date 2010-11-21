namespace Uncas.WebTester.Utilities.Tests.Unit
{
    using System.Net;
    using Moq;
    using NUnit.Framework;

    [TestFixture]
    public class BrowserUtilityTests : BrowserUtilityTestsBase
    {
        protected override IBrowserUtility GetBrowserUtility()
        {
            var mock = new Mock<IBrowserUtility>();
            var resultOK = GetNavigateResult(HttpStatusCode.OK);
            var resultNotFound = GetNavigateResult(HttpStatusCode.NotFound);
            var resultInternalServerError = GetNavigateResult(HttpStatusCode.InternalServerError);
            mock.Setup(m => m.NavigateTo(this.ExistingUrl)).Returns(resultOK);
            mock.Setup(m => m.NavigateTo(this.NonExistingUrl)).Returns(resultNotFound);
            mock.Setup(m => m.NavigateTo(this.Error500Url)).Returns(resultInternalServerError);
            mock.Setup(m => m.NavigateTo(this.PageWithEmptyLink)).Returns(resultOK);
            return mock.Object;
        }

        private static NavigateResult GetNavigateResult(
            HttpStatusCode statusCode)
        {
            return new NavigateResult(statusCode, 2, 3, 2);
        }
    }
}