#region Copyright (c) 2013 James Snape
// <copyright file="ParallelTask.cs" company="James Snape">
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
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// ParallelTask class definition.
    /// </summary>
    public class ParallelTask : CompositeTask
    {
        /// <summary>
        /// Initializes a new instance of the ParallelTask class.
        /// </summary>
        /// <param name="tasks">A sequence of tasks.</param>
        public ParallelTask(IEnumerable<ITask> tasks)
            : base(tasks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ParallelTask class.
        /// </summary>
        /// <param name="tasks">A sequence of tasks.</param>
        public ParallelTask(params ITask[] tasks)
            : this((IEnumerable<ITask>)tasks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ParallelTask class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="tasks">A sequence of tasks.</param>
        public ParallelTask(string name, IEnumerable<ITask> tasks)
            : base(name, tasks)
        {
        }

        /// <summary>
        /// Initializes a new instance of the ParallelTask class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="tasks">A sequence of tasks.</param>
        public ParallelTask(string name, params ITask[] tasks)
            : this(name, (IEnumerable<ITask>)tasks)
        {
        }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected override async Task ExecuteInternalAsync(ITaskContext context)
        {
            var waits = new List<Task>();

            foreach (var task in this.Tasks)
            {
                context.CancellationToken.ThrowIfCancellationRequested();

                waits.Add(task.ExecuteAsync(context));
            }

            await Task.WhenAll(waits);
        }
    }
}
