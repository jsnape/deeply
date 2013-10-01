#region Copyright (c) 2013 James Snape
// <copyright file="TaskFacts.cs" company="James Snape">
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
    using System.Threading.Tasks;
    using Autofac;
    using Deeply.Tests.Fakes;
    using Deeply.Tests.Fixtures;
    using FluentAssertions;
    using NSubstitute;
    using Ploeh.AutoFixture;
    using Xbehave;
    using Xunit;

    /// <summary>
    /// Task tests
    /// </summary>
    /// <remarks>This outer class is also a base class for common code.</remarks>
    public class TaskFacts : SimpleContextFixture
    {
        /// <summary>
        /// Task under test.
        /// </summary>
        private readonly FakeTask task = new FakeTask();

        /// <summary>
        /// Default construction tests
        /// </summary>
        /// <remarks>
        /// This class is an experiment with BDD and the <c>Xbehave</c> framework. I'm not convinced yet.
        /// </remarks>
        public class DefaultConstructionFacts
        {
            /// <summary>
            /// Task under test.
            /// </summary>
            private TaskBase task;

            /// <summary>
            /// Can create new tasks.
            /// </summary>
            [Background]
            public void Background()
            {
                "Given a task"
                    ._(() => this.task = new FakeTask());
            }

            /// <summary>
            /// Can create new tasks.
            /// </summary>
            [Scenario]
            public void CanCreateNewTasks()
            {
                "Then the task should not be null"
                    ._(() => this.task.Should().NotBeNull());

                "And the task name should not be empty"
                    ._(() => this.task.Name.Should().NotBeNullOrEmpty());
            }

            /// <summary>
            /// Two tasks should not have the same default name.
            /// </summary>
            /// <param name="task2">The second task to compare with.</param>
            [Scenario]
            public void TwoTasksShouldNotHaveTheSameDefaultName(ITask task2)
            {
                "And another task"
                    ._(() => task2 = new FakeTask());

                "Then the task names should not be the same"
                    ._(() => this.task.Name.Should().NotBe(task2.Name));
            }
        }

        /// <summary>
        /// Named construction tests
        /// </summary>
        public class NamedConstructionFacts : SimpleContextFixture
        {
            /// <summary>
            /// Named task should have the correct name.
            /// </summary>
            [Fact]
            public void NamedTaskShouldHaveCorrectName()
            {
                var anyString = this.Fixture.Create<string>();
                var task = new FakeTask(anyString);

                Assert.Equal(anyString, task.Name);
            }
        }
    
        /// <summary>
        /// Execution tests
        /// </summary>
        public sealed class ExecutionFacts : TaskFacts
        {
            /// <summary>
            /// Disabled tasks should not execute.
            /// </summary>
            [Fact]
            public void DisabledTasksShouldNotExecute()
            {
                this.task.Enabled = false;
                this.task.Execute(this.Context);

                Assert.False(this.task.ExecuteWasCalled);
            }

            /// <summary>
            /// Enabled tasks should execute.
            /// </summary>
            [Fact]
            public void EnabledTasksShouldExecute()
            {
                this.task.Enabled = true;
                this.task.Execute(this.Context);

                Assert.True(this.task.ExecuteWasCalled);
            }

            /// <summary>
            /// Task should throw when cancelled.
            /// </summary>
            [Fact]
            public void ThrowsWhenCanceled()
            {
                this.Context.Cancel();

                Assert.Throws<OperationCanceledException>(
                    () => this.task.Execute(this.Context));
            }

            /// <summary>
            /// Task should throw is a null context is passed to synchronous execute.
            /// </summary>
            [Fact]
            public void SyncThrowsWhenNullContextPassed()
            {
                Assert.Throws<ArgumentNullException>(() => this.task.Execute(null));
            }

            /// <summary>
            /// Task should throw is a null context is passed to asynchronous execute.
            /// </summary>
            /// <returns>A task handle used to signal.</returns>
            [Fact]
            public async Task AsyncThrowsWhenNullContextPassed()
            {
                try
                {
                    await this.task.ExecuteAsync(null);
                    Assert.True(false);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ArgumentNullException>(ex);
                }
            }
        }
    
        /// <summary>
        /// Verify tests.
        /// </summary>
        public sealed class VerifyFacts : TaskFacts
        {
            /// <summary>
            /// Disabled tasks should not verify.
            /// </summary>
            [Fact]
            public void DisabledTasksShouldNotVerify()
            {
                this.task.Enabled = false;
                this.task.Verify(this.Context);

                Assert.False(this.task.VerifyWasCalled);
            }

            /// <summary>
            /// Enabled tasks should verify.
            /// </summary>
            [Fact]
            public void EnabledTasksShouldVerify()
            {
                this.task.Enabled = true;
                this.task.Verify(this.Context);

                Assert.True(this.task.VerifyWasCalled);
            }

            /// <summary>
            /// Task should throw when cancelled.
            /// </summary>
            [Fact]
            public void ThrowsWhenCanceled()
            {
                this.Context.Cancel();

                Assert.Throws<OperationCanceledException>(
                    () => this.task.Verify(this.Context));
            }

            /// <summary>
            /// Task should throw is a null context is passed to synchronous verify.
            /// </summary>
            [Fact]
            public void SyncThrowsWhenNullContextPassed()
            {
                Assert.Throws<ArgumentNullException>(() => this.task.Verify(null));
            }

            /// <summary>
            /// Task should throw is a null context is passed to asynchronous verify.
            /// </summary>
            /// <returns>A task handle used to signal.</returns>
            [Fact]
            public async Task AsyncThrowsWhenNullContextPassed()
            {
                try
                {
                    await this.task.VerifyAsync(null);
                    Assert.True(false);
                }
                catch (Exception ex)
                {
                    Assert.IsType<ArgumentNullException>(ex);
                }
            }
        }
   
        /// <summary>
        /// Execution log tests.
        /// </summary>
        public sealed class ExecutionLogFacts
        {
            /// <summary>
            /// Log substitute.
            /// </summary>
            private IExecutionLog log = Substitute.For<IExecutionLog>();

            /// <summary>
            /// Task under test.
            /// </summary>
            private FakeTask task = new FakeTask();

            /// <summary>
            /// Task should call the start then success events in turn.
            /// </summary>
            [Fact]
            public void SucceedingTaskShouldRaiseStartThenSuccessEvents()
            {
                using (var fixture = new SimpleContextFixture(
                    b => b.RegisterInstance(this.log).As<IExecutionLog>()))
                {
                    this.task.Execute(fixture.Context);
                }

                this.log.Received(1).TaskStarted(this.task);
                this.log.Received(1).TaskSucceeded(this.task);

                this.log.DidNotReceiveWithAnyArgs().TaskFailed(null, Arg.Any<string>());
            }

            /// <summary>
            /// Failed Task should call the start then failed events in turn.
            /// </summary>
            [Fact]
            public void FailedTaskShouldRaiseStartThenFailedEvents()
            {
                this.task.ExecuteFunction =
                    c => { throw new InvalidOperationException(); };

                using (var fixture = new SimpleContextFixture(
                    b => b.RegisterInstance(this.log).As<IExecutionLog>()))
                {
                    try
                    {
                        this.task.Execute(fixture.Context);
                    }
                    catch (AggregateException)
                    {
                        // This exception type is thrown because we are executing in
                        // an asynchronous context.
                    }
                }

                this.log.Received(1).TaskStarted(this.task);
                this.log.Received(1).TaskFailed(this.task, Arg.Any<string>());

                this.log.DidNotReceiveWithAnyArgs().TaskSucceeded(null);
            }
        }
    }
}
