namespace Uncas.WebTester.NUnitRunner.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using NUnit.Framework;

    [TestFixture]
    public class NUnitCrawlerTestsZenUncasDk : NUnitCrawler
    {
        protected override IList<HttpStatusCode> AcceptableStatusCodes
        {
            get
            {
                var result = base.AcceptableStatusCodes;
                result.Add(HttpStatusCode.InternalServerError);
                return result;
            }
        }

        protected override IList<Uri> GetBaseUrls()
        {
            var result = new List<Uri>();
            result.Add(new Uri("http://zen.uncas.dk"));
            return result;
        }
    }
}