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
    using System.Threading.Tasks;
    using Deeply.Tests.Fakes;
    using Deeply.Tests.Fixtures;
    using Ploeh.AutoFixture;
    using Xunit;

    /// <summary>
    /// Task tests
    /// </summary>
    public static class TaskFacts
    {
        /// <summary>
        /// Default construction tests
        /// </summary>
        public class DefaultConstructionFacts
        {
            /// <summary>
            /// Task under test.
            /// </summary>
            private TaskBase task = new FakeTask();

            /// <summary>
            /// Can create new tasks.
            /// </summary>
            [Fact]
            public void CanCreateNewTasks()
            {
                Assert.NotNull(this.task);
            }

            /// <summary>
            /// The default task name should not be empty.
            /// </summary>
            [Fact]
            public void DefaultTaskNameShouldNotBeEmpty()
            {
                Assert.NotEqual(0, this.task.Name.Length);
            }

            /// <summary>
            /// Two tasks should not have the same default name.
            /// </summary>
            [Fact]
            public void TwoTasksShouldNotHaveTheSameDefaultName()
            {
                var task2 = new FakeTask();
                Assert.NotEqual(task2.Name, this.task.Name);
            }
        }

        /// <summary>
        /// Named construction tests
        /// </summary>
        public class NamedConstructionFacts
        {
            /// <summary>
            /// Test fixture instance.
            /// </summary>
            private Fixture fixture = new Fixture();

            /// <summary>
            /// Named task should have the correct name.
            /// </summary>
            [Fact]
            public void NamedTaskShouldHaveCorrectName()
            {
                var anyString = this.fixture.Create<string>();
                var task = new FakeTask(anyString);

                Assert.Equal(anyString, task.Name);
            }
        }
    
        /// <summary>
        /// Execution tests
        /// </summary>
        public sealed class ExecutionFacts : DefaultTaskContextFixture
        {
            /// <summary>
            /// Task under test.
            /// </summary>
            private readonly FakeTask task = new FakeTask();

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
        public sealed class VerifyFacts : DefaultTaskContextFixture
        {
            /// <summary>
            /// Task under test.
            /// </summary>
            private readonly FakeTask task = new FakeTask();

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
    }
}
