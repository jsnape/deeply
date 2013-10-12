#region Copyright (c) 2013 James Snape
// <copyright file="ResourceScope.cs" company="James Snape">
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
    /// Resource scope decorator class definition.
    /// </summary>
    public class ResourceScope : TaskBase
    {
        /// <summary>
        /// Decorated task.
        /// </summary>
        private readonly ITask decoratedTask;

        /// <summary>
        /// Sequence of resources to release.
        /// </summary>
        private readonly IEnumerable<IDisposable> resources;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceScope"/> class.
        /// </summary>
        /// <param name="decoratedTask">Wrapped task.</param>
        /// <param name="resources">Sequence of resources to dispose of.</param>
        public ResourceScope(ITask decoratedTask, params IDisposable[] resources)
            : this(TaskBase.NextTaskName(), decoratedTask, (IEnumerable<IDisposable>)resources)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceScope"/> class.
        /// </summary>
        /// <param name="decoratedTask">Wrapped task.</param>
        /// <param name="resources">Sequence of resources to dispose of.</param>
        public ResourceScope(ITask decoratedTask, IEnumerable<IDisposable> resources)
            : this(TaskBase.NextTaskName(), decoratedTask, resources)
        {    
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceScope"/> class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="decoratedTask">Wrapped task.</param>
        /// <param name="resources">Sequence of resources to dispose of.</param>
        public ResourceScope(string name, ITask decoratedTask, params IDisposable[] resources)
            : this(name, decoratedTask, (IEnumerable<IDisposable>)resources)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceScope"/> class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="decoratedTask">Wrapped task.</param>
        /// <param name="resources">Sequence of resources to dispose of.</param>
        public ResourceScope(string name, ITask decoratedTask, IEnumerable<IDisposable> resources)
            : base(name)
        {
            if (decoratedTask == null)
            {
                throw new ArgumentNullException("decoratedTask");
            }

            if (resources == null)
            {
                throw new ArgumentNullException("resources");
            }

            this.decoratedTask = decoratedTask;
            this.resources = resources;
        }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected override async Task ExecuteInternalAsync(ITaskContext context)
        {
            await this.decoratedTask.ExecuteAsync(context);

            foreach (var resource in this.resources)
            {
                resource.Dispose();
            }
        }
    }
}
