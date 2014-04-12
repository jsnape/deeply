#region Copyright (c) 2013 James Snape
// <copyright file="CsvBulkRepository.cs" company="James Snape">
//  Copyright 2014 James Snape
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

namespace Deeply.Extras
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Deeply;

    /// <summary>
    /// CSV bulk repository class definition.
    /// </summary>
    /// <typeparam name="T">Entity type to be bulk inserted.</typeparam>
    public class CsvBulkRepository<T> : IBulkRepository<T>
    {
        /// <summary>
        /// Bulk copies the supplied data into the database.
        /// </summary>
        /// <param name="rows">A row iterator</param>
        /// <param name="context">Task execution context</param>
        /// <returns>A task representing the completion of this function.</returns>
        public Task BulkCopyAsync(IEnumerable<T> rows, ITaskContext context)
        {
            return Task.FromResult(0);
        }
    }
}
