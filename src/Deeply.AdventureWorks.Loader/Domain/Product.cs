#region Copyright (c) 2013 James Snape
// <copyright file="Product.cs" company="James Snape">
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
    /// Product class definition.
    /// </summary>
    public class Product
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the product subcategory key.
        /// </summary>
        public int ProductSubcategoryKey { get; set; }

        /// <summary>
        /// Gets or sets the product subcategory alternate key.
        /// </summary>
        public int ProductSubcategoryAlternateKey { get; set; }

        /// <summary>
        /// Gets or sets the alternate key.
        /// </summary>
        public string AlternateKey { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string SpanishName
        {
            get { return this.Name; } 
        }

        /// <summary>
        /// Gets the name.
        /// </summary>
        public string FrenchName 
        {
            get { return this.Name; } 
        }
    }
}
