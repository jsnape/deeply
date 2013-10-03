#region Copyright (c) 2013 James Snape
// <copyright file="ExecuteProcessTask.cs" company="James Snape">
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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Threading.Tasks;

    /// <summary>
    /// ExecuteProcessTask class definition.
    /// </summary>
    public class ExecuteProcessTask : TaskBase
    {
        /// <summary>
        /// Default period for completion polling.
        /// </summary>
        private const int DefaultProcessPollPeriod = 10000;

        /// <summary>
        /// Process start information.
        /// </summary>
        private readonly ProcessStartInfo startInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteProcessTask"/> class.
        /// </summary>
        /// <param name="startInfo">Information used to start the process including command-line parameters.</param>
        public ExecuteProcessTask(ProcessStartInfo startInfo)
            : this(TaskBase.NextTaskName(), startInfo)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExecuteProcessTask"/> class.
        /// </summary>
        /// <param name="name">Task name.</param>
        /// <param name="startInfo">Information used to start the process including command-line parameters.</param>
        public ExecuteProcessTask(string name, ProcessStartInfo startInfo)
            : base(name)
        {
            if (startInfo == null)
            {
                throw new ArgumentNullException("startInfo");
            }

            this.startInfo = startInfo;
        }

        /// <summary>
        /// Implementation function for the execution.
        /// </summary>
        /// <remarks>This function is not asynchronous.</remarks>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this execution.</returns>
        protected override Task ExecuteInternalAsync(ITaskContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (context.CancellationToken == null)
            {
                throw new ArgumentException("context argument must contain a valid cancellation token", "context");
            }

            this.startInfo.ErrorDialog = false;
            
            var process = Process.Start(this.startInfo);
            
            process.EnableRaisingEvents = true;

            // I don't see any harm in polling here since we are already executing
            // on the thread pool and its a good time to report process.
            while (!process.WaitForExit(DefaultProcessPollPeriod))
            {
                string message = string.Format(
                    CultureInfo.CurrentCulture,
                    Properties.Resources.ReportProcessProgressFormat,
                    this.startInfo.FileName,
                    process.TotalProcessorTime);

                context.Log.ReportProgress(this, message, 50);

                if (context.CancellationToken.IsCancellationRequested)
                {
                    process.Kill();

                    context.CancellationToken.ThrowIfCancellationRequested();
                }
            }

            return TaskBase.Complete;
        }

        /// <summary>
        /// Implementation function for the verification.
        /// </summary>
        /// <param name="context">Verification context.</param>
        /// <returns>A task that represents the completion of this verification.</returns>
        protected override Task VerifyInternalAsync(ITaskContext context)
        {
            //// I'm not sure how to implement this function since the
            //// file name may not be the full path and the full path environment
            //// will be searched for a valid file.

            return TaskBase.Complete;
        }
    }
}
