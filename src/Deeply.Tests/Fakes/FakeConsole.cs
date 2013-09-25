#region Copyright (c) 2013 James Snape
// <copyright file="FakeConsole.cs" company="James Snape">
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

namespace Deeply.Tests.Fakes
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text;

    /// <summary>
    /// FakeConsole class definition.
    /// </summary>
    public sealed class FakeConsole : IDisposable
    {
        /// <summary>
        /// Cache for the old console output stream.
        /// </summary>
        private readonly TextWriter previousConsoleOut;

        /// <summary>
        /// Cache for the old console error stream.
        /// </summary>
        private readonly TextWriter previousConsoleError;

        /// <summary>
        /// String cache for the replaced console output stream.
        /// </summary>
        private readonly StringWriter outWriter = new StringWriter(CultureInfo.CurrentCulture);

        /// <summary>
        /// String cache for the replaced console error stream.
        /// </summary>
        private readonly StringWriter errorWriter = new StringWriter(CultureInfo.CurrentCulture);

        /// <summary>
        /// Initializes a new instance of the FakeConsole class.
        /// </summary>
        public FakeConsole()
        {
            this.previousConsoleOut = Console.Out;
            this.previousConsoleError = Console.Error;

            Console.SetOut(this.outWriter);
            Console.SetError(this.errorWriter);
        }

        /// <summary>
        /// Gets the current out stream value.
        /// </summary>
        public string OutValue
        {
            get
            {
                return this.outWriter.ToString();
            }
        }

        /// <summary>
        /// Gets the current error stream value.
        /// </summary>
        public string ErrorValue
        {
            get
            {
                return this.errorWriter.ToString();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Console.SetOut(this.previousConsoleOut);
            this.outWriter.Close();

            Console.SetError(this.previousConsoleError);
            this.errorWriter.Close();
        }
    }
}
