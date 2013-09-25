#region Copyright (c) 2013 James Snape
// <copyright file="CompositeTask.cs" company="James Snape">
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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Composite task class definition.
    /// </summary>
    public abstract class CompositeTask : TaskBase
    {
        /// <summary>
        /// Task sequence.
        /// </summary>
        private readonly IEnumerable<ITask> tasks;

        /// <summary>
        /// Initializes a new instance of the CompositeTask class.
        /// </summary>
        /// <param name="tasks">A sequence of tasks.</param>
        protected CompositeTask(IEnumerable<ITask> tasks)
        {
            this.tasks = tasks;
        }

        /// <summary>
        /// Initializes a new instance of the CompositeTask class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="tasks">A sequence of tasks.</param>
        protected CompositeTask(string name, IEnumerable<ITask> tasks)
            : base(name)
        {
            this.tasks = tasks;
        }

        /// <summary>
        /// Gets the child tasks.
        /// </summary>
        public IEnumerable<ITask> Tasks
        {
            get
            {
                return this.tasks;
            }
        }

        /// <summary>
        /// Implementation function for the verification.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this verification.</returns>
        protected override async Task VerifyInternal(ITaskContext context)
        {
            await base.VerifyInternal(context);

            var waits = new List<Task>();

            foreach (var task in this.Tasks)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                waits.Add(task.VerifyAsync(context));
            }

            await Task.WhenAll(waits);
        }
    }
}
