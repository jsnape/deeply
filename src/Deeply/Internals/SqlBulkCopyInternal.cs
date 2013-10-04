#region Copyright (c) 2013 James Snape
// <copyright file="SqlBulkCopyInternal.cs" company="James Snape">
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

namespace Deeply.Internals
{
    using System.Data;
    using System.Data.SqlClient;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// Internal implementation of IBulkCopy.
    /// </summary>
    internal class SqlBulkCopyInternal : IBulkCopy
    {
        /// <summary>
        /// Real bulk copy implementation.
        /// </summary>
        private readonly SqlBulkCopy bulkCopy;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlBulkCopyInternal"/> class.
        /// </summary>
        /// <param name="connectionString">Database connection string.</param>
        /// <param name="copyOptions">Bulk copy options.</param>
        public SqlBulkCopyInternal(string connectionString, SqlBulkCopyOptions copyOptions)
        {
            this.bulkCopy = new SqlBulkCopy(connectionString, copyOptions);

            // Changing this to false causes all the data to be loaded into memory before
            // sending to the server which is not a good idea for large data sets.
            this.bulkCopy.EnableStreaming = true;

            this.bulkCopy.BulkCopyTimeout = 0;
        }

        /// <summary>
        /// Gets or sets the name of the destination table on the server.
        /// </summary>
        public string DestinationTableName 
        {
            get { return this.bulkCopy.DestinationTableName; }
            set { this.bulkCopy.DestinationTableName = value; }
        }

        /// <summary>
        /// Adds a column mapping.
        /// </summary>
        /// <param name="sourceColumn">Source column.</param>
        /// <param name="destinationColumn">Destination column.</param>
        public void AddColumnMapping(string sourceColumn, string destinationColumn)
        {
            this.bulkCopy.ColumnMappings.Add(sourceColumn, destinationColumn);
        }

        /// <summary>
        /// Copies all rows in the supplied System.Data.IDataReader to a destination table.
        /// </summary>
        /// <param name="reader">A <see cref="System.Data.IDataReader"/> whose rows will be copied to the destination table.</param>
        /// <param name="cancellationToken">The cancellation instruction</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task WriteToServerAsync(IDataReader reader, CancellationToken cancellationToken)
        {
            await this.bulkCopy.WriteToServerAsync(reader, cancellationToken);
        }

        /// <summary>
        /// Handles resource disposal.
        /// </summary>
        public void Dispose()
        {
            this.bulkCopy.Close();
        }
    }
}
