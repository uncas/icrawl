//-------------
// <copyright file="ResultService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ApplicationServices
{
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Repositories;

    /// <summary>
    /// Processes results to a repository.
    /// </summary>
    public class ResultService : IResultService
    {
        /// <summary>
        /// The repository.
        /// </summary>
        private readonly IResultRepository repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResultService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public ResultService(IResultRepository repository)
        {
            this.repository = repository;
        }

        /// <summary>
        /// Processes the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        public void ProcessResult(HyperLink link)
        {
            this.repository.Add(link);            
        }
    }
}