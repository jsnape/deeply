#region Copyright (c) 2013 James Snape
// <copyright file="SqlBulkRepository.cs" company="James Snape">
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
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Threading.Tasks;
    using Deeply.Internals;

    /// <summary>
    /// <c>SqlBulkRepository</c> class definition
    /// </summary>
    /// <typeparam name="T">Entity type to be bulk inserted.</typeparam>
    public class SqlBulkRepository<T> : IBulkRepository<T>
    {
        /// <summary>
        /// Factory function for creating bulk copy implementations.
        /// </summary>
        private readonly Func<string, SqlBulkCopyOptions, IBulkCopy> createBulkCopy;

        /// <summary>
        /// The table name
        /// </summary>
        private readonly string tableName;

        /// <summary>
        /// The connection factory
        /// </summary>
        private readonly IDbConnectionFactory connectionFactory;

        /// <summary>
        /// The column mappings
        /// </summary>
        private readonly IDictionary<string, string> columnMappings;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBulkRepository{T}"/> class.
        /// </summary>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="columnMappings">The column mappings.</param>
        public SqlBulkRepository(
            string tableName, 
            IDbConnectionFactory connectionFactory, 
            IDictionary<string, string> columnMappings)
            : this((c, o) => new SqlBulkCopyInternal(c, o), tableName, connectionFactory, columnMappings)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBulkRepository{T}"/> class.
        /// </summary>
        /// <param name="createBulkCopy">Factory function for creating a bulk copy implementation.</param>
        /// <param name="tableName">Name of the table.</param>
        /// <param name="connectionFactory">The connection factory.</param>
        /// <param name="columnMappings">The column mappings.</param>
        internal SqlBulkRepository(
            Func<string, SqlBulkCopyOptions, IBulkCopy> createBulkCopy,
            string tableName, 
            IDbConnectionFactory connectionFactory, 
            IDictionary<string, string> columnMappings)
        {
            if (createBulkCopy == null)
            {
                throw new ArgumentNullException("createBulkCopy");
            }

            this.createBulkCopy = createBulkCopy;
            this.tableName = tableName;
            this.connectionFactory = connectionFactory;
            this.columnMappings = columnMappings;
        }

        /// <summary>
        /// Copies all rows in the supplied <c>IDataReader</c> to a destination table specified by the TableName constructor parameter.
        /// </summary>
        /// <param name="rows">A <c>IEnumerable</c> whose items will be copied to the destination table.</param>
        /// <param name="context">Task execution context</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task BulkCopyAsync(IEnumerable<T> rows, ITaskContext context)
        {
            if (rows == null)
            {
                throw new ArgumentNullException("rows");
            }

            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var connectionString = this.connectionFactory.ConnectionString;
            var copyOptions = SqlBulkCopyOptions.TableLock;

            using (var bulkCopy = this.createBulkCopy(connectionString, copyOptions))
            {
                bulkCopy.DestinationTableName = this.tableName;

                foreach (var key in this.columnMappings.Keys)
                {
                    bulkCopy.AddColumnMapping(key, this.columnMappings[key]);
                }

                using (var reader = new EnumerableDataReader<T>(rows))
                {
                    await bulkCopy.WriteToServerAsync(reader, context.CancellationToken);
                }
            }
        }
    }
}
