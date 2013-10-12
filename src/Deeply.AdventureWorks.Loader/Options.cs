#region Copyright (c) 2013 James Snape
// <copyright file="Options.cs" company="James Snape">
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

namespace Deeply.AdventureWorks.Loader
{
    using System.ComponentModel;

    /// <summary>
    /// Command line options holder.
    /// </summary>
    public class Options
    {
        /// <summary>
        /// Gets or sets a value indicating whether help should be shown.
        /// </summary>
        [Description("Shows command line help and options")]
        public bool Help { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether a full load should be executed.
        /// </summary>
        [Description("Performs a full load (default is for incremental)")]
        public bool FullLoad { get; set; }

        /// <summary>
        /// Gets or sets the source data path.
        /// </summary>
        [Description("Source data path")]
        [DefaultValue(@"C:\Users\James\SkyDrive\Data\AdventureWorks 2008 OLTP Script")]
        public string SourcePath { get; set; }
    }
}
