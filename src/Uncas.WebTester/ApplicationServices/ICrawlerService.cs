//-------------
// <copyright file="ICrawlerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    /// <summary>
    /// Crawls a website.
    /// </summary>
    public interface ICrawlerService
    {
        /// <summary>
        /// Crawls the website.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        void Crawl(ICrawlConfiguration configuration);
    }
}