#region Copyright (c) 2013 James Snape
// <copyright file="IExecutionLog.cs" company="James Snape">
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
    /// IExecutionLog interface definition.
    /// </summary>
    public interface IExecutionLog
    {
        /// <summary>
        /// Called when a task starts.
        /// </summary>
        /// <param name="task">The task.</param>
        void TaskStarted(ITask task);

        /// <summary>
        /// Called when a task has finished execution successfully.
        /// </summary>
        /// <param name="task">The task.</param>
        void TaskSucceeded(ITask task);

        /// <summary>
        /// Called when a task execution fails.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="reason">Task failure reason.</param>
        void TaskFailed(ITask task, string reason);

        /// <summary>
        /// Called regularly during task execution to report progress.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="message">A message of progress.</param>
        /// <param name="percentComplete">An estimation of percent complete.</param>
        void ReportProgress(ITask task, string message, int percentComplete);
    }
}
