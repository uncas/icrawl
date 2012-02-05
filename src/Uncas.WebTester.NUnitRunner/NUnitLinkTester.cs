//-------------
// <copyright file="NUnitLinkTester.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.NUnitRunner
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Net;
    using System.Text;
    using NUnit.Framework;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Crawler integration with NUnit.
    /// </summary>
    public abstract class NUnitLinkTester : IResultService
    {
        /// <summary>
        /// The default maximum number of links.
        /// </summary>
        private const int MaxVisitsDefault = 10;

        /// <summary>
        /// The object that locks things common for all tests.
        /// </summary>
        private readonly object lockObject = new object();

        /// <summary>
        /// The failed links.
        /// </summary>
        private IList<HyperLink> failedLinks = new List<HyperLink>();

        /// <summary>
        /// The crawler service.
        /// </summary>
        private ICrawlerService crawlerService;

        /// <summary>
        /// The text index.
        /// </summary>
        private int testIndex;

        /// <summary>
        /// Initializes a new instance of the <see cref="NUnitLinkTester"/> class.
        /// </summary>
        protected NUnitLinkTester()
        {
            IBrowserUtility browser = new HtmlAgilityBrowserUtility();
            this.crawlerService = new CrawlerService(this, browser);
        }

        /// <summary>
        /// Gets the acceptable status codes.
        /// </summary>
        /// <value>The acceptable status codes.</value>
        protected virtual IList<HttpStatusCode> AcceptableStatusCodes
        {
            get
            {
                var result = new List<HttpStatusCode>();
                result.Add(HttpStatusCode.OK);
                return result;
            }
        }

        /// <summary>
        /// Gets the max visits.
        /// </summary>
        /// <value>The max visits.</value>
        protected virtual int? MaxVisits
        {
            get
            {
                return MaxVisitsDefault;
            }
        }

        /// <summary>
        /// Checks the links.
        /// </summary>
        [Test]
        public void CheckLinks()
        {
            var startUrls = this.GetBaseUrls();
            ICrawlConfiguration configuration =
                new CrawlConfiguration(
                startUrls,
                this.MaxVisits);
            Console.WriteLine("Testing {0} pages on {1}:", this.MaxVisits, startUrls[0].AbsoluteUri);
            this.crawlerService.Crawl(configuration);
            if (this.failedLinks.Count > 0)
            {
                this.WriteFailure();
            }
            else
            {
                Console.WriteLine("All pages were OK!");
            }
        }

        /// <summary>
        /// Processes the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        public void ProcessResult(HyperLink link)
        {
            lock (this.lockObject)
            {
                if (!this.IsLinkOk(link))
                {
                    this.failedLinks.Add(link);
                }

                this.testIndex++;
            }
        }

        /// <summary>
        /// Gets the base url.
        /// </summary>
        /// <returns>The base url.</returns>
        [SuppressMessage(
            "Microsoft.Design",
            "CA1024:UsePropertiesWhereAppropriate",
            Justification = "This might involve a database call or other long-running calls.")]
        protected abstract IList<Uri> GetBaseUrls();

        /// <summary>
        /// Writes the failure.
        /// </summary>
        private void WriteFailure()
        {
            var messageBuilder = new StringBuilder("There were failed pages:");
            foreach (var page in this.failedLinks)
            {
                messageBuilder.AppendFormat(
                    "{0}{1}: {2}",
                    Environment.NewLine,
                    page.StatusCode,
                    page.Url.AbsoluteUri);
            }

            Assert.Fail(messageBuilder.ToString());
        }

        /// <summary>
        /// Links the should be ok.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        /// <returns>
        /// <c>true</c> if [is link ok] [the specified link]; otherwise, <c>false</c>.
        /// </returns>
        private bool IsLinkOk(HyperLink link)
        {
            bool pageIsOk =
                this.AcceptableStatusCodes.Contains(link.StatusCode);
            if (!pageIsOk)
            {
                string errorMessage = string.Format(
                    CultureInfo.CurrentCulture,
                    "Status code was {0} but was expected to be {1} for Url {2}",
                    link.StatusCode,
                    this.GetAcceptableStatusCodesString(),
                    link.Url.AbsoluteUri);
                Console.WriteLine(errorMessage);
                return false;
            }

            string successMessage = string.Format(
                CultureInfo.CurrentCulture,
                "OK: {0}/{1}: {2}",
                this.testIndex + 1,
                this.MaxVisits,
                link.Url.AbsoluteUri);
            Console.WriteLine(successMessage);
            return true;
        }

        /// <summary>
        /// Gets the acceptable status codes string.
        /// </summary>
        /// <returns>A string representing the acceptable status codes.</returns>
        private string GetAcceptableStatusCodesString()
        {
            StringBuilder result = new StringBuilder();
            foreach (var statusCode in this.AcceptableStatusCodes)
            {
                if (!string.IsNullOrEmpty(result.ToString()))
                {
                    result.Append("/");
                }

                result.Append(statusCode);
            }

            return result.ToString();
        }
    }
}