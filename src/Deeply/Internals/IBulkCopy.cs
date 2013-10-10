#region Copyright (c) 2013 James Snape
// <copyright file="IBulkCopy.cs" company="James Snape">
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
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Text;
    using System.Threading;
    using System.Threading.Tasks;

    /// <summary>
    /// IBulkCopy interface definition.
    /// </summary>
    public interface IBulkCopy : IDisposable
    {
        /// <summary>
        /// Gets or sets the name of the destination table on the server.
        /// </summary>
        string DestinationTableName { get; set; }

        /// <summary>
        /// Adds a column mapping.
        /// </summary>
        /// <param name="sourceColumn">Source column.</param>
        /// <param name="destinationColumn">Destination column.</param>
        void AddColumnMapping(string sourceColumn, string destinationColumn);

        /// <summary>
        /// Copies all rows in the supplied System.Data.IDataReader to a destination table.
        /// </summary>
        /// <param name="reader">A <see cref="System.Data.IDataReader"/> whose rows will be copied to the destination table.</param>
        /// <param name="cancellationToken">The cancellation instruction</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task WriteToServerAsync(IDataReader reader, CancellationToken cancellationToken);
    }
}
