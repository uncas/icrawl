//-------------
// <copyright file="NavigationObserver.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Utilities
{
    using System.Diagnostics.CodeAnalysis;
    using System.Net;
    using SHDocVw;
    using WatiN.Core;

    /// <summary>
    /// The navigation observer.
    /// </summary>
    internal class NavigationObserver
    {
        /// <summary>
        /// The status code.
        /// </summary>
        private HttpStatusCode statusCode = HttpStatusCode.OK;

        /// <summary>
        /// The last url.
        /// </summary>
        private object url;

        /// <summary>
        /// Initializes a new instance of the <see cref="NavigationObserver"/> class.
        /// </summary>
        /// <param name="ie">The internet explorer browser.</param>
        public NavigationObserver(IE ie)
        {
            InternetExplorer internetExplorer =
                (InternetExplorer)ie.InternetExplorer;
            internetExplorer.NavigateComplete2 +=
                new DWebBrowserEvents2_NavigateComplete2EventHandler(
                    this.InternetExplorer_NavigateComplete2);
            internetExplorer.NavigateError +=
                new DWebBrowserEvents2_NavigateErrorEventHandler(
                    this.IeNavigateError);
        }

        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>The status code.</value>
        public HttpStatusCode StatusCode
        {
            get
            {
                return this.statusCode;
            }
        }

        /// <summary>
        /// Internets the explorer_ navigate complete2.
        /// </summary>
        /// <param name="pDisp">The p disp.</param>
        /// <param name="URL">The URL of the page.</param>
        [SuppressMessage(
            "Microsoft.StyleCop.CSharp.NamingRules",
            "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Interop...")]
        [SuppressMessage(
            "Microsoft.StyleCop.CSharp.NamingRules",
            "SA1306:FieldNamesMustBeginWithLowerCaseLetter",
            Justification = "Interop...")]
        private void InternetExplorer_NavigateComplete2(
           object pDisp,
           ref object URL)
        {
            if (this.url != null && this.url.ToString() == URL.ToString())
            {
                return;
            }

            this.statusCode = HttpStatusCode.OK;
        }

        /// <summary>
        /// Handles navigate error.
        /// </summary>
        /// <param name="pDisp">The p disp.</param>
        /// <param name="URL">The URL of the page.</param>
        /// <param name="Frame">The given frame.</param>
        /// <param name="StatusCode">The status code.</param>
        /// <param name="Cancel">if set to <c>true</c> [cancel].</param>
        [SuppressMessage(
            "Microsoft.StyleCop.CSharp.NamingRules",
            "SA1305:FieldNamesMustNotUseHungarianNotation",
            Justification = "Interop...")]
        [SuppressMessage(
            "Microsoft.StyleCop.CSharp.NamingRules",
            "SA1306:FieldNamesMustBeginWithLowerCaseLetter",
            Justification = "Interop...")]
        private void IeNavigateError(
            object pDisp,
            ref object URL,
            ref object Frame,
            ref object StatusCode,
            ref bool Cancel)
        {
            this.url = URL;
            this.statusCode = (HttpStatusCode)StatusCode;
        }
    }
}