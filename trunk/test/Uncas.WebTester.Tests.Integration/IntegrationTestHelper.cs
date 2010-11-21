namespace Uncas.WebTester.Tests.Integration
{
    using System.Configuration;

    internal static class IntegrationTestHelper
    {
        public static string GetConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["webTesterTestConnectionString"].ConnectionString;
        }

        public static int GetWebsitePort()
        {
            string value = ConfigurationManager.AppSettings["website.port"] ?? "52349";
            return int.Parse(value);
        }
    }
}
