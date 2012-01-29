using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    class PageDummy
    {
        public PageDummy(int id)
        {
            this.Id = id;
        }

        public bool Visited { get; set; }

        public int Id { get; private set; }
    }
}
