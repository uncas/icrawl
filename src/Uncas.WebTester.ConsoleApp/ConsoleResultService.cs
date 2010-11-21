//-------------
// <copyright file="ConsoleResultService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ConsoleApp
{
    using System;
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Models;

    /// <summary>
    /// Processes results to the console.
    /// </summary>
    internal class ConsoleResultService : IResultService
    {
        /// <summary>
        /// Processes the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        public void ProcessResult(HyperLink link)
        {
            Console.WriteLine(
                "{0} {1}: {2}",
                (int)link.StatusCode,
                link.StatusCode,
                link.Url);
        }
    }
}
