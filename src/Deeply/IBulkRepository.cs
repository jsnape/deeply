#region Copyright (c) 2013 James Snape
// <copyright file="IBulkRepository.cs" company="James Snape">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// IReferralRepository interface definition.
    /// </summary>
    /// <typeparam name="T">Repository entity type</typeparam>
    public interface IBulkRepository<T>
    {
        /// <summary>
        /// Bulk copies the supplied data into the database.
        /// </summary>
        /// <param name="rows">A row iterator</param>
        /// <param name="context">Task execution context</param>
        /// <returns>A task representing the completion of this function.</returns>
        Task BulkCopyAsync(IEnumerable<T> rows, ITaskContext context);
    }
}
