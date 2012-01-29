//-------------
// <copyright file="CrawlerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Crawls a website.
    /// </summary>
    public class CrawlerService : ICrawlerService
    {
        /// <summary>
        /// The browser utility.
        /// </summary>
        private readonly IBrowserUtility browserUtility;

        /// <summary>
        /// The result service.
        /// </summary>
        private readonly IResultService resultService;

        /// <summary>
        /// The random number generator.
        /// </summary>
        private readonly Random random = new Random();

        /// <summary>
        /// The lock object.
        /// </summary>
        private static object lockObject = new object();

        /// <summary>
        /// Initializes a new instance of the <see cref="CrawlerService"/> class.
        /// </summary>
        /// <param name="resultService">The result service.</param>
        /// <param name="browserUtility">The browser utility.</param>
        public CrawlerService(
            IResultService resultService,
            IBrowserUtility browserUtility)
        {
            this.browserUtility = browserUtility;
            this.resultService = resultService;
        }

        /// <summary>
        /// Crawls the website.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        public void Crawl(CrawlConfiguration configuration)
        {
            Guid batchNumber = Guid.NewGuid();
            IList<HyperLink> links = GetStartUrls(configuration);

            this.ContinueLoop(links, configuration, batchNumber);

            int maxDegreeOfParallelism = configuration.MaxDegreeOfParallelism;

            if (maxDegreeOfParallelism == 1)
            {
                Console.WriteLine("Running in sequence.");
                this.RunInSequence(configuration, batchNumber, links);
            }
            else
            {
                Console.WriteLine("Running in up to {0} threads.", maxDegreeOfParallelism);
                this.RunInParallel(configuration, batchNumber, links, maxDegreeOfParallelism);
            }
        }

        /// <summary>
        /// Gets the start urls.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A list of hyper links.</returns>
        private static IList<HyperLink> GetStartUrls(
            CrawlConfiguration configuration)
        {
            IList<HyperLink> links = new List<HyperLink>();
            foreach (Uri startUrl in configuration.StartUrls)
            {
                links.Add(new HyperLink(startUrl));
            }

            return links;
        }

        /// <summary>
        /// Adds the new links.
        /// </summary>
        /// <param name="links">The links.</param>
        /// <param name="currentLinks">The links for the current page.</param>
        /// <param name="configuration">The configuration.</param>
        private static void AddNewLinks(
            IList<HyperLink> links,
            IEnumerable<HyperLink> currentLinks,
            CrawlConfiguration configuration)
        {
            var newLinks = currentLinks.Where(
                l => !links.Any(l2 => l2.Url == l.Url) &&
                l.Url != null &&
                configuration.MatchesPattern(l.Url));
            foreach (var newLink in newLinks)
            {
                links.Add(newLink);
            }
        }

        /// <summary>
        /// Runs the in sequence.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <param name="links">The links.</param>
        private void RunInSequence(
            CrawlConfiguration configuration,
            Guid batchNumber,
            IList<HyperLink> links)
        {
            for (int i = 0; i < configuration.MaxVisits - 1; i++)
            {
                this.ContinueLoop(links, configuration, batchNumber);
            }
        }

        /// <summary>
        /// Runs the in parallel.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <param name="links">The links.</param>
        /// <param name="maxDegreeOfParallelism">The max degree of parallelism.</param>
        private void RunInParallel(
            CrawlConfiguration configuration,
            Guid batchNumber,
            IList<HyperLink> links,
            int maxDegreeOfParallelism)
        {
            // Parallellized using this tip:
            // http://stackoverflow.com/questions/8671771/whats-the-best-way-of-achieving-a-parallel-infinite-loop
            var options =
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = maxDegreeOfParallelism
                };
            Parallel.For(
                0,
                configuration.MaxVisits - 1,
                options,
                (x) =>
                {
                    this.ContinueLoop(links, configuration, batchNumber);
                });
        }

        /// <summary>
        /// Continues the loop.
        /// </summary>
        /// <param name="links">The links.</param>
        /// <param name="configuration">The configuration.</param>
        /// <param name="batchNumber">The batch number.</param>
        private void ContinueLoop(
            IList<HyperLink> links,
            CrawlConfiguration configuration,
            Guid batchNumber)
        {
            IList<HyperLink> availableLinks;
            lock (lockObject)
            {
                availableLinks = links.Where(l => !l.IsVisited).ToList();
                if (availableLinks.Count() == 0)
                {
                    return;
                }
            }

            this.HandleNextLink(
                configuration,
                links,
                availableLinks,
                batchNumber);
        }

        /// <summary>
        /// Handles the next link.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="links">The links.</param>
        /// <param name="availableLinks">The available links.</param>
        /// <param name="batchNumber">The batch number.</param>
        private void HandleNextLink(
            CrawlConfiguration configuration,
            IList<HyperLink> links,
            IEnumerable<HyperLink> availableLinks,
            Guid batchNumber)
        {
            NavigateResult navigateResult =
                this.GoToNextLink(
                availableLinks,
                batchNumber);
            if (navigateResult == null)
            {
                return;
            }

            lock (lockObject)
            {
                AddNewLinks(
                    links,
                    navigateResult.Links,
                    configuration);
            }
        }

        /// <summary>
        /// Goes to the next link.
        /// </summary>
        /// <param name="availableLinks">The available links.</param>
        /// <param name="batchNumber">The batch number.</param>
        /// <returns>The navigate result.</returns>
        private NavigateResult GoToNextLink(
            IEnumerable<HyperLink> availableLinks,
            Guid batchNumber)
        {
            HyperLink nextLink;
            lock (lockObject)
            {
                int nextIndex = this.random.Next(availableLinks.Count());
                nextLink = availableLinks.ElementAt(nextIndex);
                nextLink.Visit();
            }

            NavigateResult result =
                NavigateHelper.NavigateToAndProcessUrl(
                nextLink.Url,
                this.browserUtility);
            lock (lockObject)
            {
                nextLink.UpdateWithNavigateResult(
                    result.LoadTime,
                    result.StatusCode,
                    result.NumberOfLinks,
                    result.Images,
                    result.HtmlLength,
                    result.DocumentElements,
                    batchNumber);
            }

            this.resultService.ProcessResult(nextLink);
            return result;
        }
    }
}