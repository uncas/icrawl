//-------------
// <copyright file="RandomController.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Web.Controllers
{
    using System;
    using System.Threading;
    using System.Web.Mvc;

    /// <summary>
    /// Random pages for testing.
    /// </summary>
    public class RandomController : Controller
    {
        /// <summary>
        /// Random number generator.
        /// </summary>
        private static readonly Random random = new Random();

        /// <summary>
        /// Indexes the specified id.
        /// </summary>
        /// <param name="id">The id of the random subpage.</param>
        /// <returns>A random list of links.</returns>
        public ActionResult Index(int? id)
        {
            Thread.Sleep(200);
            string content = string.Format(
@"
<a href='/Random/Index/{0}0'>0</a>
<a href='/Random/Index/{0}1'>1</a>
<a href='/Random/Index/{0}2'>2</a>
<a href='/Random/Index/{0}3'>3</a>
<a href='/Random/Index/{0}4'>4</a>
<a href='/Random/Index/{0}5'>5</a>
<a href='/Random/Index/{0}6'>6</a>
<a href='/Random/Index/{0}7'>7</a>
<a href='/Random/Index/{0}8'>8</a>
<a href='/Random/Index/{0}9'>9</a>
",
                id);
            return Content(content);
        }
    }
}
