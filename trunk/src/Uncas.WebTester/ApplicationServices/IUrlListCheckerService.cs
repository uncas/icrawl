//-------------
// <copyright file="IUrlListCheckerService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using Uncas.WebTester.Repositories;

    /// <summary>
    /// Service for checking url list.
    /// </summary>
    public interface IUrlListCheckerService
    {
        /// <summary>
        /// Checks the URL list.
        /// </summary>
        /// <param name="provider">The url list provider.</param>
        void CheckUrlList(IUrlListProvider provider);
    }
}