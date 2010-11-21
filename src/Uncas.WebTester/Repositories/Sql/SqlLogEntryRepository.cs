//-------------
// <copyright file="SqlLogEntryRepository.cs" company="Uncas">
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
    /// Sql storage for log entries.
    /// </summary>
    public class SqlLogEntryRepository : SqlHelper, ILogEntryRepository
    {
        /// <summary>
        /// The name of the table.
        /// </summary>
        private const string TableName = "WebTestLog";

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlLogEntryRepository"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlLogEntryRepository(string connectionString)
            : base(connectionString)
        {
        }

        /// <summary>
        /// Adds the specified log entry.
        /// </summary>
        /// <param name="item">The log entry.</param>
        public void Add(LogEntry item)
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
    , Message  nvarchar(max)  NOT NULL
    , Version  nvarchar(32)  NOT NULL
    , StackTrace  nvarchar(max)  NULL
    , ExceptionType  nvarchar(128)  NULL
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
        /// <param name="logEntry">The log entry.</param>
        private void SaveResult(LogEntry logEntry)
        {
            string scriptFormat =
@"INSERT INTO {0}
(Message, Version, StackTrace, ExceptionType)
VALUES
(@Message, @Version, @StackTrace, @ExceptionType)";
            string script = string.Format(
                CultureInfo.InvariantCulture,
                scriptFormat,
                TableName);
            this.ExecuteCommand(
                script,
                new SqlParameter("Message", logEntry.Message),
                new SqlParameter("Version", LogEntry.Version),
                BaseSqlAdo.GetNvarcharParameter("StackTrace", logEntry.StackTrace),
                BaseSqlAdo.GetNvarcharParameter("ExceptionType", logEntry.ExceptionType));
        }
    }
}
