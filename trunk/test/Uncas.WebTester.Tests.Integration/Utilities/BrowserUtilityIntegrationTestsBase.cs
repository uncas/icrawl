namespace Uncas.WebTester.Utilities.Tests.Integration
{
    using NUnit.Framework;
    using Uncas.WebTester.Tests.Integration;
    using Uncas.WebTester.Utilities.Tests.Unit;

    [TestFixture]
    public abstract class BrowserUtilityIntegrationTestsBase :
        BrowserUtilityTestsBase
    {
        protected override int WebsitePort
        {
            get
            {
                return IntegrationTestHelper.GetWebsitePort();
            }
        }
    }
}