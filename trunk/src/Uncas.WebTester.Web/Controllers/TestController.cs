//-------------
// <copyright file="TestController.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Web.Controllers
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Web.Mvc;

    /// <summary>
    /// Handles requests for test pages.
    /// </summary>
    public class TestController : Controller
    {
        /// <summary>
        /// Returns a page with an empty link.
        /// </summary>
        /// <returns>The page with an empty link.</returns>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This is an action result and needs to be non-static")]
        public ActionResult EmptyLink()
        {
            return Content("<html><head><title>Page with empty link</title></head><body><a href=''>Hello</a></body></html>", "text/html");
        }

        /// <summary>
        /// Returns a 500 error page.
        /// </summary>
        /// <returns>An internal server error.</returns>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This is an action result and needs to be non-static")]
        public ActionResult Error500()
        {
            throw new NotImplementedException(
                "This is an internal server error");
        }

        /// <summary>
        /// Returns a valid page.
        /// </summary>
        /// <returns>An valid page.</returns>
        [SuppressMessage(
            "Microsoft.Performance",
            "CA1822:MarkMembersAsStatic",
            Justification = "This is an action result and needs to be non-static")]
        public ActionResult ValidPage()
        {
            return Content("Example of valid page");
        }
    }
}