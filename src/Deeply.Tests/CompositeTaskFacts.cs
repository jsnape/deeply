#region Copyright (c) 2013 James Snape
// <copyright file="CompositeTaskFacts.cs" company="James Snape">
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
    using System.Collections.Generic;
    using System.Threading;
    using System.Threading.Tasks;
    using Deeply.Tests.Fakes;
    using Deeply.Tests.Fixtures;
    using Xunit;

    /// <summary>
    /// CompositeTaskFacts Task tests.
    /// </summary>
    public class CompositeTaskFacts : SimpleContextFixture
    {
        /// <summary>
        /// Number of tasks used in each sequence.
        /// </summary>
        private const int TaskCount = 10;

        /// <summary>
        /// Task name.
        /// </summary>
        private const string TaskName = "VERIFY ONLY";

        /// <summary>
        /// Number of executing tasks.
        /// </summary>
        private int executingCount = 0;

        /// <summary>
        /// Maximum number of executing tasks.
        /// </summary>
        private int maxExecuting = 0;

        /// <summary>
        /// Test tasks.
        /// </summary>
        private ITask[] tasks;

        /// <summary>
        /// Initializes a new instance of the CompositeTaskFacts class.
        /// </summary>
        public CompositeTaskFacts()
        {
            this.tasks = new ITask[TaskCount];

            for (int i = 0; i < TaskCount; ++i)
            {
                var task = new FakeTask();

                task.ExecuteFunction = this.TaskCounterFunction;
                task.VerifyFunction = this.TaskCounterFunction;

                this.tasks[i] = task;
            }
        }

        /// <summary>
        /// Null task sequence throws.
        /// </summary>
        [Fact]
        public static void NullTaskSequenceThrows()
        {
            var exception = Assert.Throws<ArgumentNullException>(() => new SequentialTask((IEnumerable<ITask>)null));
            Assert.Equal("tasks", exception.ParamName);
        }

        /// <summary>
        /// Sequential Tasks execute in sequence.
        /// </summary>
        /// <returns>A task that represents the completion of this execution.</returns>
        [Fact]
        public async Task SequentialTasksExecuteInSequence()
        {
            var task = new SequentialTask(this.tasks);

            await task.ExecuteAsync(this.Context);

            Assert.Equal(1, this.maxExecuting);
        }

        /// <summary>
        /// Sequential Tasks verify in parallel.
        /// </summary>
        /// <returns>A task that represents the completion of this execution.</returns>
        [Fact]
        public async Task SequentialTasksVerifyInParallel()
        {
            var task = new SequentialTask(TaskName, this.tasks);
                
            await task.VerifyAsync(this.Context);

            Assert.True(this.maxExecuting > 1);
        }

        /// <summary>
        /// Task constructed by name have the correct name..
        /// </summary>
        [Fact]
        public void SequentialTaskNameConstructorSetsName()
        {
            var task = new SequentialTask(TaskName, this.tasks);

            Assert.Equal(TaskName, task.Name);
        }

        /// <summary>
        /// Parallel Tasks execute in parallel.
        /// </summary>
        /// <returns>A task that represents the completion of this execution.</returns>
        [Fact]
        public async Task ParallelTasksExecuteInParallel()
        {
            var task = new ParallelTask(this.tasks);

            await task.ExecuteAsync(this.Context);

            Assert.True(this.maxExecuting > 1);
        }

        /// <summary>
        /// Parallel Tasks verify in parallel.
        /// </summary>
        /// <returns>A task that represents the completion of this execution.</returns>
        [Fact]
        public async Task ParallelTasksVerifyInParallel()
        {
            var task = new ParallelTask(TaskName, this.tasks);

            await task.VerifyAsync(this.Context);

            Assert.True(this.maxExecuting > 1);
        }

        /// <summary>
        /// Task constructed by name have the correct name..
        /// </summary>
        [Fact]
        public void ParallelTaskNameConstructorSetsName()
        {
            var task = new ParallelTask(TaskName, this.tasks);

            Assert.Equal(TaskName, task.Name);
        }

        /// <summary>
        /// Helper function to increment and decrement a counter.
        /// </summary>
        /// <param name="context">Execution context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        private async Task TaskCounterFunction(ITaskContext context)
        {
            Interlocked.Increment(ref this.executingCount);

            Interlocked.CompareExchange(ref this.maxExecuting, this.executingCount, this.executingCount - 1);

            // This Yield function must remain as we need to give other tasks
            // the chance to start executing, hence the possibility of incorrectly 
            // running in parallel which will be tested for.
            await Task.Yield();

            Interlocked.Decrement(ref this.executingCount);
        }
    }
}
