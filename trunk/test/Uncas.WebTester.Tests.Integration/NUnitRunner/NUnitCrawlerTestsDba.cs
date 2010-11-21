namespace Uncas.WebTester.NUnitRunner.Tests
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class NUnitCrawlerTestsDba : NUnitCrawler
    {
        protected override int? MaxLinks
        {
            get
            {
                return 10;
            }
        }

        protected override IList<Uri> GetBaseUrls()
        {
            var result = new List<Uri>();
            result.Add(new Uri("http://www.dba.dk"));
            return result;
        }
    }
}