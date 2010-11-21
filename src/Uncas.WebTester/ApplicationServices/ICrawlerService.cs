//-------------
// <copyright file="ICrawlerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System.Collections.Generic;
    using Uncas.WebTester.Models;

    /// <summary>
    /// Crawls a website.
    /// </summary>
    public interface ICrawlerService
    {
        /// <summary>
        /// Crawls the website.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Crawl(CrawlConfiguration configuration);

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <returns>A list of hyper links.</returns>
        IEnumerable<HyperLink> GetLinks(CrawlConfiguration configuration);
    }
}