namespace Uncas.WebTester.NUnitRunner.Tests.Integration
{
    using System;
    using System.Collections.Generic;
    using NUnit.Framework;

    [TestFixture]
    public class NUnitLinkTesterUncasDk : NUnitLinkTester
    {
        protected override IList<Uri> GetBaseUrls()
        {
            return new Uri[] { new Uri("http://www.uncas.dk") };
        }
    }
}