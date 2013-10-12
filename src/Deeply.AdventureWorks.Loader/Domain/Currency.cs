#region Copyright (c) 2013 James Snape
// <copyright file="Currency.cs" company="James Snape">
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

namespace Deeply.AdventureWorks.Loader.Domain
{
    /// <summary>
    /// Currency entity definition.
    /// </summary>
    public class Currency
    {
        /// <summary>
        /// Gets or sets the currency key.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the currency alternate key.
        /// </summary>
        public string AlternateKey { get; set; }

        /// <summary>
        /// Gets or sets the currency name.
        /// </summary>
        public string Name { get; set; }
    }
}
