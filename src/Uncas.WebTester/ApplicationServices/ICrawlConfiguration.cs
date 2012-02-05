//-------------
// <copyright file="ICrawlConfiguration.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// The crawl configuration.
    /// </summary>
    public interface ICrawlConfiguration
    {
        /// <summary>
        /// Gets the match patterns.
        /// </summary>
        /// <value>The match patterns.</value>
        IEnumerable<string> MatchPatterns { get; }

        /// <summary>
        /// Gets the max degree of parallelism.
        /// </summary>
        /// <value>The max degree of parallelism.</value>
        int MaxDegreeOfParallelism { get; }

        /// <summary>
        /// Gets the max number of pages to visit.
        /// </summary>
        /// <value>The max number of pages to visit.</value>
        int MaxVisits { get; }

        /// <summary>
        /// Gets the start urls.
        /// </summary>
        /// <value>The start urls.</value>
        IList<Uri> StartUrls { get; }

        /// <summary>
        /// Adds the matches.
        /// </summary>
        /// <param name="matchList">The match list.</param>
        void AddMatches(IEnumerable<string> matchList);

        /// <summary>
        /// Matches the pattern.
        /// </summary>
        /// <param name="url">The given URL.</param>
        /// <returns>True if any pattern matches, otherwise false.</returns>
        bool MatchesPattern(Uri url);
    }
}
