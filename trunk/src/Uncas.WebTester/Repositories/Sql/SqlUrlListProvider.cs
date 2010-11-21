//-------------
// <copyright file="SqlUrlListProvider.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Repositories.Sql
{
    using System;
    using System.Globalization;
    using Uncas.WebTester.Models;

    /// <summary>
    /// Sql provider for url list.
    /// </summary>
    public class SqlUrlListProvider : SqlHelper, IUrlListProvider
    {
        /// <summary>
        /// The name of the table.
        /// </summary>
        private const string TableName = "WebTestUrl";

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlUrlListProvider"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlUrlListProvider(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Gets the next nextLink.
        /// </summary>
        /// <returns>The next nextLink.</returns>
        public HyperLink GetNextLink()
        {
            if (!this.DatabaseIsCreated)
            {
                this.CreateDatabase();
                this.CreateTable();
            }

            const string SqlCommandTextFormat =
@"
UPDATE {0}
SET Visited = GETDATE()
OUTPUT inserted.Url
WHERE Id IN
(
    SELECT TOP 1 Id FROM {0} 
    WHERE Visited IS NULL
    ORDER BY Id ASC
)";
            string sqlCommandText = string.Format(
                CultureInfo.InvariantCulture,
                SqlCommandTextFormat,
                TableName);
            string urlString = (string)ExecuteScalar(sqlCommandText);
            if (string.IsNullOrEmpty(urlString))
            {
                return null;
            }

            return new HyperLink(new Uri(urlString));
        }

        /// <summary>
        /// Creates the table.
        /// </summary>
        private void CreateTable()
        {
            string tableScriptFormat =
@"IF NOT EXISTS
(
    SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{0}'
)
CREATE TABLE {0}
(
    Id  bigint  NOT NULL  IDENTITY(1,1)
        CONSTRAINT PK_{0} PRIMARY KEY CLUSTERED
    , Created  datetime  NOT NULL
        CONSTRAINT DF_{0}_Created DEFAULT GETDATE()
    , Url  nvarchar(max)  NOT NULL
    , Visited  datetime  NULL
)";
            string tableScript = string.Format(
                CultureInfo.InvariantCulture,
                tableScriptFormat,
                TableName);
            this.ExecuteCommand(tableScript);
        }
    }
}