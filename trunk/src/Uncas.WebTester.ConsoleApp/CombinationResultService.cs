//-------------
// <copyright file="CombinationResultService.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.ConsoleApp
{
    using Uncas.WebTester.ApplicationServices;
    using Uncas.WebTester.Models;
    using Uncas.WebTester.Repositories.Sql;

    /// <summary>
    /// Combines results for console and sql.
    /// </summary>
    public class CombinationResultService : IResultService
    {
        /// <summary>
        /// The console result service.
        /// </summary>
        private readonly ConsoleResultService consoleResultService;

        /// <summary>
        /// The sql result service.
        /// </summary>
        private readonly ResultService sqlResultService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CombinationResultService"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string for the sql result service.</param>
        public CombinationResultService(string connectionString)
        {
            this.consoleResultService = new ConsoleResultService();
            var resultRepository = new SqlResultRepository(connectionString);
            this.sqlResultService = new ResultService(resultRepository);
        }

        /// <summary>
        /// Processes the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        public void ProcessResult(HyperLink link)
        {
            this.consoleResultService.ProcessResult(link);
            this.sqlResultService.ProcessResult(link);
        }
    }
}