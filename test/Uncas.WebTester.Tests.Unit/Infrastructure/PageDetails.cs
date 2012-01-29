namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    using System;
    using System.Collections.Generic;

    public class PageDetails
    {
        private static readonly Random random = new Random();

        private readonly IList<PageDummy> links;

        public PageDetails()
        {
            this.links = new List<PageDummy>();
        }

        public IEnumerable<PageDummy> Links
        {
            get { return this.links; }
        }

        public void AddLink()
        {
            this.links.Add(new PageDummy(random.Next()));
        }
    }
}
