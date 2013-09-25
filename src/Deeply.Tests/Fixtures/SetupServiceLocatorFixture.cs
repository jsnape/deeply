#region Copyright (c) 2013 James Snape
// <copyright file="SetupServiceLocatorFixture.cs" company="James Snape">
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

namespace Deeply.Tests.Fixtures
{
    using Microsoft.Practices.ServiceLocation;

    /// <summary>
    /// SetupServiceLocatorFixture class definition.
    /// </summary>
    public class SetupServiceLocatorFixture
    {
        /// <summary>
        /// Set once to 
        /// </summary>
        private bool initialized;

        /// <summary>
        /// Configures the service locator.
        /// </summary>
        /// <remarks>This uses a rather simplistic concurrency mechanism that is OK for testing.</remarks>
        public void ConfigureServiceLocator()
        {
            if (this.initialized)
            {
                return;
            }

            this.initialized = true;

            ServiceLocator.SetLocatorProvider(() => new DefaultServiceLocator());
        }
    }
}
