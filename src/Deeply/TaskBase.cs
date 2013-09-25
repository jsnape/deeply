#region Copyright (c) 2013 James Snape
// <copyright file="TaskBase.cs" company="James Snape">
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

namespace Deeply
{
    using System;
    using System.Globalization;
    using System.Threading.Tasks;

    /// <summary>
    /// Task class definition.
    /// </summary>
    public abstract class TaskBase : ITask
    {
        /// <summary>
        /// Helper instance for async functions that are never await.
        /// </summary>
        private static readonly Task CompleteTask = Task.FromResult(0);

        /// <summary>
        /// Next task id.
        /// </summary>
        private static int nextId = 0;

        /// <summary>
        /// Task name.
        /// </summary>
        private readonly string name;

        /// <summary>
        /// Initializes a new instance of the TaskBase class with a generated name.
        /// </summary>
        protected TaskBase()
        {
            this.Enabled = true;
            this.name = string.Format(CultureInfo.CurrentCulture, "Task{0}", ++nextId);
        }

        /// <summary>
        /// Initializes a new instance of the TaskBase class with the supplied name.
        /// </summary>
        /// <param name="name">Task name.</param>
        protected TaskBase(string name)
        {
            this.Enabled = true;
            this.name = name;
        }

        /// <summary>
        /// Gets the task name.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this task is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets a task instance that has completed.
        /// </summary>
        protected static Task Complete
        {
            get
            {
                return TaskBase.CompleteTask;
            }
        }

        /// <summary>
        /// Executes the task synchronously.
        /// </summary>
        /// <remarks>The default implementation just calls <c>ExecuteAsync</c> and waits for the call to complete.</remarks>
        /// <param name="context">Execution context.</param>
        public void Execute(ITaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.ExecuteAsync(context).Wait(context.CancellationToken);
        }

        /// <summary>
        /// Executes the task asynchronously.
        /// </summary>
        /// <param name="context">Execution context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        public async Task ExecuteAsync(ITaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!this.Enabled)
            {
                return;
            }

            context.CancellationToken.ThrowIfCancellationRequested();

            await this.ExecuteInternal(context);
        }

        /// <summary>
        /// Verifies the task synchronously.
        /// </summary>
        /// <remarks>The default implementation just calls <c>VerifyAsync</c> and waits for the call to complete.</remarks>
        /// <param name="context">Verification context.</param>
        public void Verify(ITaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            this.VerifyAsync(context).Wait(context.CancellationToken);
        }

        /// <summary>
        /// Verifies the task asynchronously.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this verification.</returns>
        public async Task VerifyAsync(ITaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (!this.Enabled)
            {
                return;
            }

            context.CancellationToken.ThrowIfCancellationRequested();

            await this.VerifyInternal(context);
        }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected abstract Task ExecuteInternal(ITaskContext context);

        /// <summary>
        /// Implementation function for the verification.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this verification.</returns>
        protected virtual Task VerifyInternal(ITaskContext context)
        {
            return Complete;
        }
    }
}
