//-------------
// <copyright file="NavigateResult.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using Uncas.WebTester.Models;

    /// <summary>
    /// The navigate result.
    /// </summary>
    public class NavigateResult
    {
        /// <summary>
        /// The hyper links.
        /// </summary>
        private ICollection<HyperLink> links;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigateResult"/> class.
        /// </summary>
        /// <param name="statusCode">The status code.</param>
        /// <param name="documentElements">The document elements.</param>
        /// <param name="htmlLength">Length of the HTML.</param>
        /// <param name="images">The images.</param>
        public NavigateResult(
            HttpStatusCode statusCode,
            int documentElements,
            int htmlLength,
            int images)
            : this()
        {
            this.DocumentElements = documentElements;
            this.HtmlLength = htmlLength;
            this.Images = images;
            this.StatusCode = statusCode;
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="NavigateResult"/> class from being created.
        /// </summary>
        private NavigateResult()
        {
            this.links = new List<HyperLink>();
        }

        /// <summary>
        /// Gets the empty navigate result.
        /// </summary>
        /// <value>The empty navigate result.</value>
        public static NavigateResult Empty
        {
            get
            {
                return new NavigateResult();
            }
        }

        /// <summary>
        /// Gets the links.
        /// </summary>
        /// <value>The hyper links.</value>
        public IEnumerable<HyperLink> Links
        {
            get
            {
                return this.links;
            }
        }

        /// <summary>
        /// Gets the load time.
        /// </summary>
        /// <value>The load time.</value>
        public TimeSpan LoadTime { get; private set; }

        /// <summary>
        /// Gets the number of links.
        /// </summary>
        /// <value>The number of links.</value>
        public int NumberOfLinks
        {
            get
            {
                return this.links.Count;
            }
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode { get; private set; }

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <value>The images.</value>
        public int Images { get; private set; }

        /// <summary>
        /// Gets the length of the text.
        /// </summary>
        /// <value>The length of the text.</value>
        public int HtmlLength { get; private set; }

        /// <summary>
        /// Gets the document elements.
        /// </summary>
        /// <value>The document elements.</value>
        public int DocumentElements { get; private set; }

        /// <summary>
        /// Adds the nextLink.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        public void AddLink(HyperLink link)
        {
            this.links.Add(link);
        }

        /// <summary>
        /// Sets the load time.
        /// </summary>
        /// <param name="loadTime">The load time.</param>
        public void AddLoadTime(TimeSpan loadTime)
        {
            this.LoadTime = loadTime;
        }
    }
}