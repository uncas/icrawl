namespace Uncas.WebTester.Tests.SimpleTestProject
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using NUnit.Framework;
    using Uncas.WebTester.Infrastructure;
    using Uncas.WebTester.NUnitRunner;

    [TestFixture]
    public class LinkTesterTest : NUnitLinkTester
    {
        protected override int? MaxVisits
        {
            get
            {
                return ConfigFileParser.GetInt32Value("MaxLinks", 10);
            }
        }

        protected override IList<HttpStatusCode> AcceptableStatusCodes
        {
            get
            {
                string[] statusCodeStrings =
                    ConfigFileParser.GetConfigValue(
                    "AcceptableStatusCodes", "200").
                    Split(',', ';');
                var result = new List<HttpStatusCode>();
                foreach (string statusCodeString in statusCodeStrings)
                {
                    var statusCode =
                        (HttpStatusCode)Enum.Parse(
                        typeof(HttpStatusCode),
                        statusCodeString);
                    result.Add(statusCode);
                }

                return result;
            }
        }

        protected override IList<Uri> GetBaseUrls()
        {
            var result = new List<Uri>();
            string[] baseUrlStrings =
                ConfigFileParser.GetConfigValue(
                "BaseUrls", "http://www.uncas.dk").
                Split(',', ';');
            foreach (string baseUrlString in baseUrlStrings)
            {
                result.Add(new Uri(baseUrlString));
            }

            return result;
        }
    }
}