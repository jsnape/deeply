#region Copyright (c) 2013 James Snape
// <copyright file="ExecuteProcessTaskFacts.cs" company="James Snape">
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

namespace Deeply.Tests
{
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using Deeply.Tests.Fixtures;
    using FluentAssertions;
    using Xunit;

    /// <summary>
    /// ExecuteProcessTask facts.
    /// </summary>
    public class ExecuteProcessTaskFacts : SimpleContextFixture
    {
        /// <summary>
        /// An invalid process.
        /// </summary>
        private const string InvalidProcess = "c:\\doesntexist.exe";

        /// <summary>
        /// A valid process.
        /// </summary>
        private const string ValidProcess = "cmd.exe";
        
        /// <summary>
        /// Process arguments.
        /// </summary>
        private const string ValidProcessArgs = "/c \"cd \\ && dir\"";

        /// <summary>
        /// Valid process start info.
        /// </summary>
        private readonly ProcessStartInfo validStartInfo = new ProcessStartInfo
        {
            FileName = ValidProcess,
            Arguments = ValidProcessArgs,
            WindowStyle = ProcessWindowStyle.Hidden
        };

        /// <summary>
        /// Should not throw an exception when verifying an invalid process.
        /// </summary>
        [Fact]
        public void VerifyingValidProcessShouldNotThrow()
        {
            var task = new ExecuteProcessTask(this.validStartInfo);

            task.Invoking(t => t.Verify(this.Context)).ShouldNotThrow();
        }

        /// <summary>
        /// Should throw an exception when verifying an invalid process.
        /// </summary>
        [Fact(Skip = "Unsure how to implement the function.")]
        public void VerifyingInvalidProcessShouldThrow()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(InvalidProcess);

            var task = new ExecuteProcessTask(startInfo);

            task.Invoking(t => t.Verify(this.Context)).ShouldThrow<FileNotFoundException>();
        }

        /// <summary>
        /// Should not throw an exception when executing a valid process.
        /// </summary>
        [Fact]
        public void ExecuteValidProcessShouldNotThrow()
        {
            var task = new ExecuteProcessTask(this.validStartInfo);

            task.Invoking(t => t.Execute(this.Context)).ShouldNotThrow();
        }

        /// <summary>
        /// Should throw an exception when executing an invalid process.
        /// </summary>
        [Fact(Skip = "Unsure how to implement the function.")]
        public void ExecuteInvalidProcessShouldThrow()
        {
            ProcessStartInfo startInfo = new ProcessStartInfo(InvalidProcess);

            var task = new ExecuteProcessTask(startInfo);

            task.Invoking(t => t.Execute(this.Context)).ShouldThrow<Win32Exception>();
        }
    }
}
