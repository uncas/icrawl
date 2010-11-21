//-------------
// <copyright file="IEBrowser.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Utilities
{
    using System.Net;
    using WatiN.Core;

    /// <summary>
    /// Wrapper for the Internet Explorer browser.
    /// </summary>
    internal class IEBrowser : IE
    {
        /// <summary>
        /// The navigation observer.
        /// </summary>
        private NavigationObserver observer;

        /// <summary>
        /// Initializes a new instance of the <see cref="IEBrowser"/> class.
        /// </summary>
        public IEBrowser()
        {
            this.observer = new NavigationObserver(this);
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.observer.StatusCode;
            }
        }
    }
}
