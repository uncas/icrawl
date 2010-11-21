namespace Uncas.WebTester.Utilities.Tests.Integration
{
    using System;
    using NUnit.Framework;
    using Uncas.WebTester.WatiNBrowserUtility;

    [TestFixture]
    [Ignore("These do not run properly on the build server")]
    public class BrowserUtilityTests : BrowserUtilityIntegrationTestsBase, IDisposable
    {
        private BrowserUtility browserUtility = new BrowserUtility();

        public void Dispose()
        {
            this.browserUtility.Dispose();
        }

        protected override IBrowserUtility GetBrowserUtility()
        {
            return this.browserUtility;
        }
    }
}