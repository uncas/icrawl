//-------------
// <copyright file="NUnitLinkTester.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.NUnitRunner
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using System.Linq;
    using System.Net;
    using System.Text;
    using System.Threading;
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
        /// Contains all links
        /// </summary>
        private readonly ConcurrentStack<HyperLink> allLinks = new ConcurrentStack<HyperLink>();

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
        /// Get links for check
        /// </summary>
        /// <returns></returns>
        public IEnumerable<TestCaseData> GetLinks()
        {
            //return new TestCaseData[]{ new TestCaseData(new HyperLink(new Uri("http://www.google.dk")))};
            var startUrls = this.GetBaseUrls();
            ICrawlConfiguration configuration =
                new CrawlConfiguration(
                startUrls,
                this.MaxVisits);
            Console.WriteLine("Testing {0} pages on {1}:", this.MaxVisits, startUrls[0].AbsoluteUri);
            this.crawlerService.Crawl(configuration);
            return this.allLinks.Select(x => new TestCaseData(x));
        }

        /// <summary>
        /// Checks the links.
        /// </summary>
        [TestCaseSource("GetLinks")]
        public void CheckLinks(HyperLink link)
        {
            Assert.That(IsLinkOk(link), "{0}: {1}", link.StatusCode, link.Url.AbsoluteUri);
        }

        /// <summary>
        /// Processes the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        public void ProcessResult(HyperLink link)
        {
            this.allLinks.Push(link);
            Interlocked.Increment(ref this.testIndex);
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