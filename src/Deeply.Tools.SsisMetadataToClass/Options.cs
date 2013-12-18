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

namespace Deeply.Tools.SsisMetadataToClass
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
        /// Gets or sets the indent width.
        /// </summary>
        [Description("The output file indent width")]
        [DefaultValue(4)]
        public int IndentWidth { get; set; }

        /// <summary>
        /// Gets or sets the output file.
        /// </summary>
        [Description("Destination file. Set to CONS for console output else a file name.")]
        [DefaultValue(@"CONS")]
        public string OutputFile { get; set; }

        /// <summary>
        /// Gets or sets the metadata source.
        /// </summary>
        [Description("Metadata source. Set to CLIP for clipboard (default), CONS for console input else a file name.")]
        [DefaultValue(@"CLIP")]
        public string MetadataSource { get; set; }

        /// <summary>
        /// Gets or sets the output class name.
        /// </summary>
        [Description("Output class name")]
        [DefaultValue(@"SsisClass")]
        public string ClassName { get; set; }

        /// <summary>
        /// Gets or sets the output class namespace.
        /// </summary>
        [Description("Output class namespace")]
        [DefaultValue(@"Replace.This")]
        public string NamespaceName { get; set; }
    }
}
