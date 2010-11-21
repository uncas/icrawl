namespace Uncas.WebTester.Repositories.Sql.Tests.Integration
{
    using NUnit.Framework;
    using Uncas.WebTester.Tests.Integration;

    [TestFixture]
    public class SqlHelperTests
    {
        [Test]
        public void CreateDatabase_CalledTwice_Ok()
        {
            var tester = new SqlHelperTester();
            tester.CreateDatabaseTwice();
        }

        private class SqlHelperTester : SqlHelper
        {
            public SqlHelperTester()
                : base(IntegrationTestHelper.GetConnectionString())
            {
            }

            public void CreateDatabaseTwice()
            {
                this.CreateDatabase();
                this.CreateDatabase();
            }
        }
    }
}
