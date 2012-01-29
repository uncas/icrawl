using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    class PageDetails
    {
        private readonly IList<PageDummy> _links;
        public PageDetails()
        {
            _links = new List<PageDummy>();
        }

        private static readonly Random _random = new Random();

        internal void AddLink()
        {
            _links.Add(new PageDummy(_random.Next()));
        }

        public IEnumerable<PageDummy> Links { get { return _links; } }
    }
}
