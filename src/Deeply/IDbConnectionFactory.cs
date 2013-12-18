#region Copyright (c) 2013 James Snape
// <copyright file="IDbConnectionFactory.cs" company="James Snape">
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
    using System.Data;

    /// <summary>
    /// <c>IDbConnectionFactory</c> interface definition.
    /// </summary>
    public interface IDbConnectionFactory
    {
        /// <summary>
        /// Gets the underlying connection string.
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Creates a new database connection instance.
        /// </summary>
        /// <returns>A connection.</returns>
        IDbConnection CreateConnection();

        /// <summary>
        /// Attempts to make a connection to the underlying database.
        /// </summary>
        /// <remarks>
        /// This value is cached to avoid multiple connections to
        /// the same destination.
        /// </remarks>
        /// <exception cref="System.Data.Common.DbException">If the connection attempt fails.</exception>
        void Validate();
    }
}
