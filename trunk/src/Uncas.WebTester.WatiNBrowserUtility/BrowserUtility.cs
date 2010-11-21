//-------------
// <copyright file="BrowserUtility.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.WatiNBrowserUtility
{
    using System;
    using System.Diagnostics;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Handles web browser functionality.
    /// </summary>
    public class BrowserUtility : IBrowserUtility, IDisposable
    {
        /// <summary>
        /// The browser (Internet Explorer).
        /// </summary>
        private readonly IEBrowser browser;

        /// <summary>
        /// Initializes a new instance of the <see cref="BrowserUtility"/> class.
        /// </summary>
        public BrowserUtility()
        {
            this.browser = new IEBrowser();
        }

        /// <summary>
        /// Navigates to the given URL.
        /// </summary>
        /// <param name="url">The URL to navigate to.</param>
        /// <returns>The navigate result.</returns>
        public NavigateResult NavigateTo(Uri url)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            this.browser.GoTo(url);
            stopwatch.Stop();
            
            // TODO: Demeter:
            var result = new NavigateResult(
                this.browser.StatusCode,
                this.browser.Elements.Count,
                this.browser.Html.Length,
                this.browser.Images.Count);
            result.AddLoadTime(stopwatch.Elapsed);
            foreach (var link in this.browser.Links)
            {
                if (string.IsNullOrEmpty(link.Url))
                {
                    continue;
                }

                result.AddLink(new HyperLink(new Uri(link.Url), url));
            }

            return result;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>True</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.browser.Close();
                this.browser.Dispose();
            }
        }
    }
}