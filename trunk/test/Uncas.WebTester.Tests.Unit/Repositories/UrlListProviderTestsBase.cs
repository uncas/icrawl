namespace Uncas.WebTester.Repositories.Tests.Unit
{
    using NUnit.Framework;

    [TestFixture]
    public abstract class UrlListProviderTestsBase
    {
        protected IUrlListProvider Provider { get; private set; }

        [SetUp]
        public void BeforeEach()
        {
            this.Provider = this.GetProvider();
        }

        [TearDown]
        public void AfterEach()
        {
            this.Provider = null;
        }

        [Test]
        public void GetNextLink()
        {
            // Act:
            this.Provider.GetNextLink();
        }

        protected abstract IUrlListProvider GetProvider();
    }
}