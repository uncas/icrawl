namespace Uncas.WebTester.Repositories.Sql.Tests.Integration
{
    using NUnit.Framework;
    using Uncas.WebTester.Repositories.Tests.Unit;
    using Uncas.WebTester.Tests.Integration;

    [TestFixture]
    public class SqlLogEntryRepositoryTests : LogEntryRepositoryTestsBase
    {
        protected override ILogEntryRepository GetRepository()
        {
            string connectionString =
                IntegrationTestHelper.GetConnectionString();
            return new SqlLogEntryRepository(connectionString);
        }
    }
}
