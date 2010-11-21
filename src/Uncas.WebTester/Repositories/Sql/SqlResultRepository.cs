//-------------
// <copyright file="SqlResultRepository.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Repositories.Sql
{
    using System.Data.SqlClient;
    using System.Globalization;
    using Uncas.WebTester.Infrastructure;
    using Uncas.WebTester.Models;

    /// <summary>
    /// Processes results to a sql database.
    /// </summary>
    public class SqlResultRepository : SqlHelper, IResultRepository
    {
        /// <summary>
        /// The name of the table.
        /// </summary>
        private const string TableName = "WebTestResults";

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlResultRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlResultRepository(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(HyperLink item)
        {
            if (!this.DatabaseIsCreated)
            {
                this.CreateDatabase();
                this.CreateTable();
            }

            this.SaveResult(item);
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
    , BatchNumber  uniqueidentifier  NOT NULL
    , Url  nvarchar(max)  NOT NULL
    , StatusCode  int  NOT NULL
    , LoadMilliseconds  int  NOT NULL
    , Links  int  NOT NULL
    , Images  int  NOT NULL
    , HtmlLength  int  NOT NULL
    , DocumentElements  int  NOT NULL
    , Referrer  nvarchar(max)  NULL
)";
            string tableScript = string.Format(
                CultureInfo.InvariantCulture,
                tableScriptFormat,
                TableName);
            this.ExecuteCommand(tableScript);
        }

        /// <summary>
        /// Saves the result.
        /// </summary>
        /// <param name="link">The hyper link.</param>
        private void SaveResult(HyperLink link)
        {
            string scriptFormat =
@"INSERT INTO {0}
(BatchNumber, Url, StatusCode, LoadMilliseconds, Links, Images, HtmlLength, DocumentElements, Referrer)
VALUES
(@BatchNumber, @Url, @StatusCode, @LoadMilliseconds, @Links, @Images, @HtmlLength, @DocumentElements, @Referrer)";
            string script = string.Format(
                CultureInfo.InvariantCulture,
                scriptFormat,
                TableName);
            this.ExecuteCommand(
                script,
                new SqlParameter("BatchNumber", link.BatchNumber),
                new SqlParameter("Url", link.Url.AbsoluteUri),
                new SqlParameter("StatusCode", (int)link.StatusCode),
                new SqlParameter("LoadMilliseconds", link.LoadTime.TotalMilliseconds),
                new SqlParameter("Links", link.NumberOfLinks),
                new SqlParameter("Images", link.Images),
                new SqlParameter("HtmlLength", link.HtmlLength),
                new SqlParameter("DocumentElements", link.DocumentElements),
                BaseSqlAdo.GetNvarcharParameter("Referrer", link.Referrer != null ? link.Referrer.AbsoluteUri : null));
        }
    }
}