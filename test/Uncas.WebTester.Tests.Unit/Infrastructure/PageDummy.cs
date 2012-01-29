namespace Uncas.WebTester.Tests.Unit.Infrastructure
{
    public class PageDummy
    {
        public PageDummy(int id)
        {
            this.Id = id;
        }

        public bool Visited { get; set; }

        public int Id { get; private set; }
    }
}
