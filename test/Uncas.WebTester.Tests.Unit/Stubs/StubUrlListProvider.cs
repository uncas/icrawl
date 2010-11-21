namespace Uncas.WebTester.Tests.Unit.Stubs
{
    using System;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Repositories;

    internal class StubUrlListProvider : IUrlListProvider
    {
        private readonly int numberOfLinks;

        private int linksRead = 0;

        public StubUrlListProvider(int numberOfLinks)
        {
            this.numberOfLinks = numberOfLinks;
        }

        public HyperLink GetNextLink()
        {
            if (this.linksRead >= this.numberOfLinks)
            {
                return null;
            }

            this.linksRead++;
            return new HyperLink(new Uri("http://www.google.dk"));
        }
    }
}
