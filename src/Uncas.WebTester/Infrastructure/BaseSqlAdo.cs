//-------------
// <copyright file="BaseSqlAdo.cs" company="Uncas">
//     Copyright (c) Ole Lynge Sørensen. All rights reserved.
// </copyright>
//-------------

namespace Uncas.WebTester.Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Data.SqlClient;

    /// <summary>
    /// Contains base sql stuff.
    /// </summary>
    public static class BaseSqlAdo
    {
        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <param name="parameters">The parameters.</param>
        public static void ExecuteCommand(
            string connectionString,
            string sqlCommandText,
            params SqlParameter[] parameters)
        {
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(sqlCommandText, connection))
                {
                    AddParameters(command, parameters);
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Executes the command.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="sqlCommandText">The SQL command text.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns>The scalar result.</returns>
        public static object ExecuteScalar(
            string connectionString,
            string sqlCommandText,
            params SqlParameter[] parameters)
        {
            object result = null;
            using (SqlConnection connection =
                new SqlConnection(connectionString))
            {
                using (SqlCommand command =
                    new SqlCommand(sqlCommandText, connection))
                {
                    AddParameters(command, parameters);
                    connection.Open();
                    result = command.ExecuteScalar();
                }
            }

            if (result is DBNull)
            {
                result = null;
            }

            return result;
        }

        /// <summary>
        /// Gets the nvarchar parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">The value for the parameter.</param>
        /// <returns>The nvarchar parameter.</returns>
        public static SqlParameter GetNvarcharParameter(
            string name,
            string value)
        {
            var result = new SqlParameter();
            result.ParameterName = name;
            if (string.IsNullOrEmpty(value))
            {
                result.Value = DBNull.Value;
            }
            else
            {
                result.Value = value;
            }

            return result;
        }

        /// <summary>
        /// Adds the parameters.
        /// </summary>
        /// <param name="command">The command.</param>
        /// <param name="parameters">The parameters.</param>
        private static void AddParameters(
            SqlCommand command,
            IEnumerable<SqlParameter> parameters)
        {
            foreach (var parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
        }
    }
}
