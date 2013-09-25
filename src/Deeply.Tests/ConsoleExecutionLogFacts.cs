#region Copyright (c) 2013 James Snape
// <copyright file="ConsoleExecutionLogFacts.cs" company="James Snape">
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
    using System;
    using Deeply.Tests.Fakes;
    using Xunit;

    /// <summary>
    /// ConsoleExecutionLog tests
    /// </summary>
    public class ConsoleExecutionLogFacts
    {
        /// <summary>
        /// Fake task.
        /// </summary>
        private FakeTask task = new FakeTask("FAKE TASK");

        /// <summary>
        /// Helper routine to setup the log.
        /// </summary>
        /// <param name="logFunc">Logging function under test.</param>
        /// <returns>The string logged in uppercase.</returns>
        public static string LogHarness(Action<IExecutionLog> logFunc)
        {
            if (logFunc == null)
            {
                throw new ArgumentNullException("logFunc");
            }

            using (var fakeConsole = new FakeConsole())
            {
                var log = new ConsoleExecutionLog();
                logFunc(log);

                var logged = fakeConsole.OutValue;
                return logged.ToUpperInvariant();
            }
        }

        /// <summary>
        /// Task Started throws then null argument passed.
        /// </summary>
        [Fact]
        public static void TaskStartedThrowsOnNullTask()
        {
            Assert.Throws<ArgumentNullException>(() => LogHarness(l => l.TaskStarted(null)));
        }

        /// <summary>
        /// Task Failed throws then null argument passed.
        /// </summary>
        [Fact]
        public static void TaskFailedThrowsOnNullTask()
        {
            Assert.Throws<ArgumentNullException>(() => LogHarness(l => l.TaskFailed(null, string.Empty)));
        }

        /// <summary>
        /// Task Succeeded throws then null argument passed.
        /// </summary>
        [Fact]
        public static void TaskSucceededThrowsOnNullTask()
        {
            Assert.Throws<ArgumentNullException>(() => LogHarness(l => l.TaskSucceeded(null)));
        }

        /// <summary>
        /// Task Progress throws then null argument passed.
        /// </summary>
        [Fact]
        public static void TaskProgressThrowsOnNullTask()
        {
            Assert.Throws<ArgumentNullException>(() => LogHarness(l => l.ReportProgress(null, string.Empty, 0)));
        }

        /// <summary>
        /// Task Started logs correct string.
        /// </summary>
        [Fact]
        public void TaskStartedLogsCorrectString()
        {
            Assert.True(LogHarness(l => l.TaskStarted(this.task)).Contains("STARTED"));
        }

        /// <summary>
        /// Task Started logs task name.
        /// </summary>
        [Fact]
        public void TaskStartedLogsTaskName()
        {
            Assert.True(LogHarness(l => l.TaskStarted(this.task)).Contains(this.task.Name));
        }

        /// <summary>
        /// Task Succeeded logs correct string.
        /// </summary>
        [Fact]
        public void TaskSucceededLogsCorrectString()
        {
            Assert.True(LogHarness(l => l.TaskSucceeded(this.task)).Contains("SUCCEEDED"));
        }

        /// <summary>
        /// Task Succeeded logs task name.
        /// </summary>
        [Fact]
        public void TaskSucceededLogsTaskName()
        {
            Assert.True(LogHarness(l => l.TaskSucceeded(this.task)).Contains(this.task.Name));
        }

        /// <summary>
        /// Task Failed throws then null reason passed.
        /// </summary>
        [Fact]
        public void TaskFailedThrowsOnNullReason()
        {
            Assert.Throws<ArgumentNullException>(() => LogHarness(l => l.TaskFailed(this.task, null)));
        }

        /// <summary>
        /// Task Failed logs correct string.
        /// </summary>
        [Fact]
        public void TaskFailedLogsCorrectString()
        {
            Assert.True(LogHarness(l => l.TaskFailed(this.task, string.Empty)).Contains("FAILED"));
        }

        /// <summary>
        /// Task Failed logs task name.
        /// </summary>
        [Fact]
        public void TaskFailedLogsTaskName()
        {
            Assert.True(LogHarness(l => l.TaskFailed(this.task, string.Empty)).Contains(this.task.Name));
        }

        /// <summary>
        /// Task Progress throws then null reason passed.
        /// </summary>
        [Fact]
        public void TaskProgressThrowsOnNullMessage()
        {
            Assert.Throws<ArgumentNullException>(() => LogHarness(l => l.ReportProgress(this.task, null, 0)));
        }

        /// <summary>
        /// Task Progress logs correct string.
        /// </summary>
        [Fact]
        public void TaskProgressLogsCorrectString()
        {
            var progressMessage = "!£$%^&*()PROGRESS";
            Assert.True(LogHarness(l => l.ReportProgress(this.task, progressMessage, 0)).Contains(progressMessage));
        }

        /// <summary>
        /// Task Progress logs task name.
        /// </summary>
        [Fact]
        public void TaskProgressLogsTaskName()
        {
            Assert.True(LogHarness(l => l.ReportProgress(this.task, string.Empty, 0)).Contains(this.task.Name));
        }
    }
}
