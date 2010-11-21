//-------------
// <copyright file="SqlHelper.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Repositories.Sql
{
    using System.Data.SqlClient;
    using System.Globalization;
    using Uncas.WebTester.Infrastructure;

    /// <summary>
    /// Sql helper.
    /// </summary>
    public abstract class SqlHelper
    {
        /// <summary>
        /// The connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlHelper"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        protected SqlHelper(string connectionString)
        {
            this.connectionString = connectionString;
        }

        /// <summary>
        /// Gets a value indicating whether the database has been created.
        /// </summary>
        /// <value><c>True</c> if [database is created]; otherwise, <c>false</c>.</value>
        protected bool DatabaseIsCreated { get; private set; }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <param name="parameters">The parameters.</param>
        protected void ExecuteCommand(
            string sqlCommandText,
            params SqlParameter[] parameters)
        {
            BaseSqlAdo.ExecuteCommand(
                this.connectionString,
                sqlCommandText,
                parameters);
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The result.</returns>
        protected object ExecuteScalar(
            string sqlCommandText,
            params SqlParameter[] parameters)
        {
            return BaseSqlAdo.ExecuteScalar(
                this.connectionString,
                sqlCommandText,
                parameters);
        }

        /// <summary>
        /// Creates the database.
        /// </summary>
        protected void CreateDatabase()
        {
            if (this.DatabaseIsCreated)
            {
                return;
            }

            SqlConnectionStringBuilder builder =
                new SqlConnectionStringBuilder(this.connectionString);
            string databaseName = builder.InitialCatalog;
            string databaseScriptFormat =
@"IF NOT EXISTS
(
    SELECT * FROM sys.databases WHERE name = '{0}'
)
CREATE DATABASE {0}";
            string databaseScript = string.Format(
                CultureInfo.InvariantCulture,
                databaseScriptFormat,
                databaseName);
            builder.InitialCatalog = "master";
            BaseSqlAdo.ExecuteCommand(builder.ConnectionString, databaseScript);
            this.DatabaseIsCreated = true;
        }
    }
}