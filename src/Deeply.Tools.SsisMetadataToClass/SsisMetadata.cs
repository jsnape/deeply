#region Copyright (c) 2013 James Snape
// <copyright file="SsisMetadata.cs" company="James Snape">
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
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// SSIS Metadata definition.
    /// </summary>
    public class SsisMetadata
    {
        /// <summary>
        /// Gets or sets the column name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the column data type.
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the column precision.
        /// </summary>
        public int Precision { get; set; }

        /// <summary>
        /// Gets or sets the column scale.
        /// </summary>
        public int Scale { get; set; }

        /// <summary>
        /// Gets or sets the column length.
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// Gets or sets the column code page.
        /// </summary>
        public int CodePage { get; set; }

        /// <summary>
        /// Gets the CLR type.
        /// </summary>
        public string ClrType
        {
            get
            {
                switch (this.DataType)
                {
                    case "DT_BOOL":
                        return "bool";

                    case "DT_BYTES":
                    case "DT_IMAGE":
                        return "byte[]";

                    case "DT_DATE":
                    case "DT_DBDATE":
                    case "DT_DBTIME":
                    case "DT_DBTIME2":
                    case "DT_DBTIMESTAMP":
                    case "DT_DBTIMESTAMP2":
                    case "DT_FILETIME":
                        return "DateTime";

                    case "DT_DBTIMESTAMPOFFSET":
                        return "DateTimeOffset";

                    case "DT_CY":
                    case "DT_DECIMAL":
                    case "DT_NUMERIC":
                        return "decimal";
                    
                    case "DT_GUID":
                        return "Guid";

                    case "DT_I1":
                        return "sbyte";

                    case "DT_I2":
                        return "short";

                    case "DT_I4":
                        return "int";

                    case "DT_I8":
                        return "long";
                    
                    case "DT_R4":
                        return "single";

                    case "DT_R8":
                        return "double";

                    case "DT_UI1":
                        return "byte";

                    case "DT_UI2":
                        return "ushort";

                    case "DT_UI4":
                        return "uint";

                    case "DT_UI8":
                        return "ulong";

                    case "DT_STR":
                    case "DT_WSTR":
                    case "DT_NTEXT":
                    case "DT_TEXT":
                        return "string";
                        
                    default:
                        var message = string.Format(
                            CultureInfo.CurrentCulture, 
                            "Unknown SSIS data type '{0}'", 
                            this.DataType);

                        throw new InvalidOperationException(message);
                }
            }
        }
    }
}
