#region Copyright (c) 2013 James Snape
// <copyright file="SimpleDataflowTask.cs" company="James Snape">
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
    using System.Threading.Tasks;

    /// <summary>
    /// SimpleDataflowTask class definition.
    /// </summary>
    /// <remarks>
    /// This class is designed for the simplest data flows i.e. those
    /// that are linear in nature with a single mapping function
    /// which converts from the source to target type.
    /// </remarks>
    /// <typeparam name="TSource">Source entity type.</typeparam>
    /// <typeparam name="TTarget">Target entity type.</typeparam>
    public class SimpleDataflowTask<TSource, TTarget> : DataflowTask
    {
        /// <summary>
        /// Source sequence.
        /// </summary>
        private IEnumerable<TSource> source;

        /// <summary>
        /// Mapping function.
        /// </summary>
        private Func<TSource, TTarget> map;

        /// <summary>
        /// Destination repository.
        /// </summary>
        private IBulkRepository<TTarget> destination;

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataflowTask{TSource, TTarget}"/> class.
        /// </summary>
        /// <param name="source">Source sequence.</param>
        /// <param name="map">Mapping function.</param>
        /// <param name="destination">Destination repository.</param>
        public SimpleDataflowTask(
            IEnumerable<TSource> source, 
            Func<TSource, TTarget> map, 
            IBulkRepository<TTarget> destination)
            : this(TaskBase.NextTaskName(), source, map, destination)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SimpleDataflowTask{TSource, TTarget}"/> class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="source">Source sequence.</param>
        /// <param name="map">Mapping function.</param>
        /// <param name="destination">Destination repository.</param>
        public SimpleDataflowTask(
            string name, 
            IEnumerable<TSource> source, 
            Func<TSource, TTarget> map, 
            IBulkRepository<TTarget> destination)
            : base(name)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (map == null)
            {
                throw new ArgumentNullException("map");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            this.source = source;
            this.map = map;
            this.destination = destination;
        }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected override async Task ExecuteInternalAsync(ITaskContext context)
        {
            await this.destination.BulkCopyAsync(
                this.source.Select(s => this.map(s)),
                context);
        }
    }
}
