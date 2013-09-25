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

namespace Deeply
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    
    /// <summary>
    /// Provides a registration point for dependency resolvers that implement IDependencyResolver.
    /// </summary>
    /// <remarks>This is the same as <see cref="System.Web.Mvc.DependencyResolver"/>.</remarks>
    public static class DependencyResolver
    {
        /// <summary>
        /// Default Dependency Resolver implementation.
        /// </summary>
        private class DefaultDependencyResolver : IDependencyResolver
        {
            /// <summary>
            /// Get service delegate.
            /// </summary>
            private readonly Func<Type, object> getService;

            /// <summary>
            /// Get services delegate.
            /// </summary>
            private readonly Func<Type, IEnumerable<object>> getServices;

            /// <summary>
            /// Initializes a new instance of the DefaultDependencyResolver class
            /// </summary>
            /// <param name="getService">The service delegate.</param>
            /// <param name="getServices">The services delegates.</param>
            public DefaultDependencyResolver(
                Func<Type, object> getService, 
                Func<Type, IEnumerable<object>> getServices)
            {
                if (getService == null)
                {
                    throw new ArgumentNullException("getService");
                }

                if (getServices == null)
                {
                    throw new ArgumentNullException("getServices");
                }

                this.getService = getService;
                this.getServices = getServices;
            }

            /// <summary>
            /// Resolves singly registered services that support arbitrary object creation.
            /// </summary>
            /// <param name="serviceType">The type of the requested service or object.</param>
            /// <returns>The requested service or object.</returns>
            public object GetService(Type serviceType)
            {
                return this.getService(serviceType);
            }

            /// <summary>
            /// Resolves multiply registered services.
            /// </summary>
            /// <param name="serviceType">The type of the requested services.</param>
            /// <returns>The requested services.</returns>
            public IEnumerable<object> GetServices(Type serviceType)
            {
                return this.getServices(serviceType);
            }
        }

        /// <summary>
        /// Gets the current dependency resolver.
        /// </summary>
        public static IDependencyResolver Current { get; private set; }

        /// <summary>
        /// Provides a registration point for dependency resolvers, using the specified dependency resolver interface.
        /// </summary>
        /// <param name="resolver">The dependency resolver.</param>
        public static void SetResolver(IDependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            if (resolver is IDisposable)
            {
                throw new InvalidOperationException("Disposable resolvers are not (yet) supported");
            }

            Current = resolver;
        }

        /// <summary>
        /// Provides a registration point for dependency resolvers using the specified service delegate and specified service collection delegates.
        /// </summary>
        /// <param name="getService">The service delegate.</param>
        /// <param name="getServices">The services delegates.</param>
        public static void SetResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
        {
            SetResolver(new DefaultDependencyResolver(getService, getServices));
        }
    }
}
