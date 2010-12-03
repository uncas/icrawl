namespace Uncas.WebTester.Models.Tests.Unit
{
    using System;
    using System.Net;
    using NUnit.Framework;

    [TestFixture]
    public class HyperLinkTests
    {
        [Test]
        public void HyperLink_WithReferrer_Ok()
        {
            var url = new Uri("http://www.uncas.dk");
            var hl = new HyperLink(url, url);
        }

        [Test]
        public void HyperLink_ToString_Ok()
        {
            var url = new Uri("http://www.uncas.dk");
            var hl = new HyperLink(url);
            hl.ToString();
        }

        [Test]
        public void UpdateStatusCode_ToBadGateway_ChangedToBadGateway()
        {
            var url = new Uri("http://www.uncas.dk");
            var hl = new HyperLink(url);
            var statusCode = HttpStatusCode.BadGateway;
            hl.UpdateStatusCode(statusCode);
            Assert.AreEqual(statusCode, hl.StatusCode);
        }
    }
}
