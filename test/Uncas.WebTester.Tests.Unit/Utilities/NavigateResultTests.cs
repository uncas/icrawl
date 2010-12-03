namespace Uncas.WebTester.Utilities.Tests
{
    using System;
    using System.Net;
    using NUnit.Framework;

    [TestFixture]
    public class NavigateResultTests
    {
        [Test]
        public void AddLoadTime_2Seconds_IsAdded()
        {
            var navigateResult =
                new NavigateResult(
                HttpStatusCode.OK, 3, 2, 1);
            var loadTime = TimeSpan.FromSeconds(2d);

            navigateResult.AddLoadTime(loadTime);

            Assert.AreEqual(loadTime, navigateResult.LoadTime);
        }
    }
}