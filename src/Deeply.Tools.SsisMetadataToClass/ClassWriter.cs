#region Copyright (c) 2013 James Snape
// <copyright file="ClassWriter.cs" company="James Snape">
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
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class writer definition.
    /// </summary>
    public class ClassWriter
    {
        /// <summary>
        /// Output options.
        /// </summary>
        private readonly Options options;

        /// <summary>
        /// Output writer.
        /// </summary>
        private readonly TextWriter output;

        /// <summary>
        /// Header format text.
        /// </summary>
        private readonly string headerFormat;

        /// <summary>
        /// Current indent width.
        /// </summary>
        private readonly int indentWidth;

        /// <summary>
        /// Current indent level.
        /// </summary>
        private int indent;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClassWriter"/> class.
        /// </summary>
        /// <param name="options">Output options.</param>
        /// <param name="output">Output writer to send class to.</param>
        /// <param name="headerFormat">Header format text.</param>
        public ClassWriter(Options options, TextWriter output, string headerFormat)
        {
            if (options == null)
            {
                throw new ArgumentNullException("options");
            }

            this.options = options;

            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            this.output = output;

            if (headerFormat == null)
            {
                throw new ArgumentNullException("headerFormat");
            }

            this.headerFormat = headerFormat;

            this.indentWidth = options.IndentWidth;
        }

        /// <summary>
        /// Gets the current indent string.
        /// </summary>
        private string Indent
        {
            get { return string.Empty.PadRight(this.indent * this.indentWidth); }
        }

        /// <summary>
        /// Helper function to setup the class and write the output.
        /// </summary>
        /// <param name="options">Output options.</param>
        /// <param name="metadata">Sequence of metadata to write.</param>
        /// <param name="output">Output writer to send class to.</param>
        public static void Write(Options options, IEnumerable<SsisMetadata> metadata, TextWriter output)
        {
            string headerFormat = Properties.Settings.Default.HeaderTemplate;

            var writer = new ClassWriter(options, output, headerFormat);
            writer.Write(metadata);
        }

        /// <summary>
        /// Writes the current sequence to the specified output.
        /// </summary>
        /// <param name="metadata">Sequence of metadata to write.</param>
        public void Write(IEnumerable<SsisMetadata> metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException("metadata");
            }

            this.WriteHeader(this.options.ClassName);

            this.WriteBeginNamespace(this.options.NamespaceName);
            ++this.indent;

            this.WriteUsing();
            this.WriteBeginClass(this.options.ClassName);
            ++this.indent;

            this.WriteMetadata(metadata);
            
            --this.indent;
            this.WriteEndClass();
            
            --this.indent;
            this.WriteEndNamespace();
        }

        /// <summary>
        /// Writes the metadata as class properties.
        /// </summary>
        /// <param name="metadata">Metadata to write.</param>
        private void WriteMetadata(IEnumerable<SsisMetadata> metadata)
        {
            bool newlineNeeded = false;

            foreach (var item in metadata)
            {
                if (newlineNeeded)
                {
                    this.output.WriteLine();
                }

                this.WriteProperty(item);

                newlineNeeded = true;
            }
        }

        /// <summary>
        /// Writes a single property.
        /// </summary>
        /// <param name="item">Property metadata</param>
        private void WriteProperty(SsisMetadata item)
        {
            this.output.WriteLine("{0}/// <summary>", this.Indent);
            this.output.WriteLine("{0}/// Gets or sets the {1}", this.Indent, item.Name);
            this.output.WriteLine("{0}/// </summary>", this.Indent);
            this.output.WriteLine("{0}public {1} {2} {{ get; set; }}", this.Indent, item.ClrType, item.Name);
        }

        /// <summary>
        /// Writes the file header.
        /// </summary>
        /// <param name="className">Name of the class to be written.</param>
        private void WriteHeader(string className)
        {
            this.output.WriteLine(this.headerFormat, className);
        }

        /// <summary>
        /// Writes a begin namespace.
        /// </summary>
        /// <param name="namespaceName">The class namespace.</param>
        private void WriteBeginNamespace(string namespaceName)
        {
            this.output.WriteLine("namespace {0}", namespaceName);
            this.output.WriteLine("{");
        }

        /// <summary>
        /// Writes the end namespace.
        /// </summary>
        private void WriteEndNamespace()
        {
            this.output.WriteLine("}");
        }

        /// <summary>
        /// Writes the using statements.
        /// </summary>
        private void WriteUsing()
        {
            this.output.WriteLine("{0}using System;", this.Indent);
            this.output.WriteLine();
        }

        /// <summary>
        /// Writes a begin class definition.
        /// </summary>
        /// <param name="className">Name of the class to be written.</param>
        private void WriteBeginClass(string className)
        {
            this.output.WriteLine("{0}/// <summary>", this.Indent);
            this.output.WriteLine("{0}/// <see cref=\"{1}\"/> class definition.", this.Indent, className);
            this.output.WriteLine("{0}/// </summary>", this.Indent);
            this.output.WriteLine("{0}public partial class {1}", this.Indent, className);
            this.output.WriteLine("{0}{{", this.Indent);
        }

        /// <summary>
        /// Writes the end of class.
        /// </summary>
        private void WriteEndClass()
        {
            this.output.WriteLine(this.Indent + "}");
        }
    }
}
