namespace Uncas.WebTester.Utilities.Tests.Integration
{
    using NUnit.Framework;

    [TestFixture]
    public class HtmlAgilityBrowserUtilityTests : BrowserUtilityIntegrationTestsBase
    {
        protected override IBrowserUtility GetBrowserUtility()
        {
            return new HtmlAgilityBrowserUtility();
        }
    }
}