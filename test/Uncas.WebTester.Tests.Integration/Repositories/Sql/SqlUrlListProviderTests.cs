namespace Uncas.WebTester.Repositories.Sql.Tests.Integration
{
    using NUnit.Framework;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Repositories.Tests.Unit;
    using Uncas.WebTester.Tests.Integration;

    [TestFixture]
    public class SqlUrlListProviderTests : UrlListProviderTestsBase
    {
        [Test]
        public void GetNextLink_LinkExists_LinkIsReturned()
        {
            // Arrange:
            new SqlUrlListProviderTester().AddUrlToList();

            // Act:
            HyperLink link = this.Provider.GetNextLink();

            // Assert:
            Assert.That(link, Is.Not.Null);
        }

        protected override IUrlListProvider GetProvider()
        {
            string connectionString =
                IntegrationTestHelper.GetConnectionString();
            return new SqlUrlListProvider(connectionString);
        }

        private class SqlUrlListProviderTester : SqlHelper
        {
            public SqlUrlListProviderTester()
                : base(IntegrationTestHelper.GetConnectionString())
            {
            }

            public void AddUrlToList()
            {
                string sql =
                    @"INSERT INTO WebTestUrl (Url) VALUES ('http://www.google.com')";
                ExecuteCommand(sql);
            }
        }
    }
}
