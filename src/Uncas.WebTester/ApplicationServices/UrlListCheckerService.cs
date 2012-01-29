//-------------
// <copyright file="UrlListCheckerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using System;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Repositories;
    using Uncas.WebTester.Utilities;

    /// <summary>
    /// Checks the url list.
    /// </summary>
    public class UrlListCheckerService : IUrlListCheckerService
    {
        /// <summary>
        /// The browser utility.
        /// </summary>
        private readonly IBrowserUtility browserUtility;

        /// <summary>
        /// The result service.
        /// </summary>
        private readonly IResultService resultService;

        /// <summary>
        /// Initializes a new instance of the <see cref="UrlListCheckerService"/> class.
        /// </summary>
        /// <param name="browserUtility">The browser utility.</param>
        /// <param name="resultService">The result service.</param>
        public UrlListCheckerService(
            IBrowserUtility browserUtility,
            IResultService resultService)
        {
            this.resultService = resultService;
            this.browserUtility = browserUtility;
        }

        /// <summary>
        /// Checks the URL list.
        /// </summary>
        /// <param name="provider">The url list provider.</param>
        public void CheckUrlList(IUrlListProvider provider)
        {
            Guid batchNumber = Guid.NewGuid();
            while (true)
            {
                HyperLink link = provider.GetNextLink();
                if (link == null)
                {
                    break;
                }

                link.UpdateBatchNumber(batchNumber);
                NavigateHelper.NavigateToAndProcessLink(
                    link,
                    this.browserUtility);
                this.resultService.ProcessResult(link);
            }
        }
    }
}