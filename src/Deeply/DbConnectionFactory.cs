#region Copyright (c) 2013 James Snape
// <copyright file="DbConnectionFactory.cs" company="James Snape">
//  Copyright 2013 James Snape
// 
//  Licensed under the Apache License, Version 2.0 (the "License");
//  you may not use this file except in compliance with the License.
//  You may obtain a copy of the License at
// 
//  http://www.apache.org/licenses/LICENSE-2.0
// 
//  Unless required by applicable law or agreed to in writing, software
//  distributed under the License is distributed on an "AS IS" BASIS,
//  WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//  See the License for the specific language governing permissions and
//  limitations under the License.
// </copyright>
#endregion

namespace Deeply
{
    using System;
    using System.Data;
    using System.Data.Common;
    using System.Data.SqlClient;

    /// <summary>
    /// ConnectionFactory class definition.
    /// </summary>
    public class DbConnectionFactory : IDbConnectionFactory
    {
        /// <summary>
        /// Connection string.
        /// </summary>
        private readonly string connectionString;

        /// <summary>
        /// Cached exception raised by validation.
        /// </summary>
        private DbException exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="DbConnectionFactory"/> class.
        /// </summary>
        /// <param name="connectionString">Connection string for all connections.</param>
        public DbConnectionFactory(string connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException("connectionString");
            }

            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string cannot be empty", "connectionString");
            }

            this.connectionString = connectionString;
        }

        /// <summary>
        /// Creates a new database connection instance.
        /// </summary>
        /// <returns>A connection.</returns>
        public IDbConnection CreateConnection()
        {
            return new SqlConnection(this.connectionString);
        }

        /// <summary>
        /// Attempts to make a connection to the underlying database.
        /// </summary>
        /// <remarks>
        /// This value is cached to avoid multiple connections to
        /// the same destination.
        /// </remarks>
        /// <exception cref="DbException">If the connection attempt fails.</exception>
        public void Validate()
        {
            if (this.exception != null)
            {
                throw this.exception;
            }

            using (var connection = this.CreateConnection())
            {
                try
                {
                    connection.Open();
                }
                catch (DbException ex)
                {
                    this.exception = ex;
                    throw;
                }
            }
        }
    }
}
