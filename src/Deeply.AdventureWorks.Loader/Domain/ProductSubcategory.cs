#region Copyright (c) 2013 James Snape
// <copyright file="ProductSubcategory.cs" company="James Snape">
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Product Subcategory class definition
    /// </summary>
    public class ProductSubcategory
    {
        /// <summary>
        /// Gets or sets the key.
        /// </summary>
        public int Key { get; set; }

        /// <summary>
        /// Gets or sets the product category key.
        /// </summary>
        public int ProductCategoryKey { get; set; }

        /// <summary>
        /// Gets or sets the product category alternate key.
        /// </summary>
        public int ProductCategoryAlternateKey { get; set; }

        /// <summary>
        /// Gets or sets the alternate key.
        /// </summary>
        public int AlternateKey { get; set; }

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
