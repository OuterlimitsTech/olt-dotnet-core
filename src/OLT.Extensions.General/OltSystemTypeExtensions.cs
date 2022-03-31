﻿using System.Collections;
using System.Linq;

// ReSharper disable once CheckNamespace
namespace System
{
    public static class OltSystemTypeExtensions
    {
        /// <summary>
        /// Determines if a type implements the <typeparamref name="TInterface"/> interface.        
        /// </summary>
        /// <typeparam name="TInterface">Interface</typeparam>
        /// <param name="type">Extends <see cref="Type"/>.</param>
        /// <example>
        /// <see href="https://gist.github.com/chrisstraw/212626e76c9eafedec24df4c2d106cea"/>
        /// </example>      
        /// <returns>True if type implements interface</returns>
        public static bool Implements<TInterface>(this Type type) where TInterface : class
        {
            var interfaceType = typeof(TInterface);

            if (!interfaceType.IsInterface)
                throw new InvalidOperationException("Only interfaces can be 'implemented'.");

            return (interfaceType.IsAssignableFrom(type));
        }

        /// <summary>
        /// Determines if a type implements the <typeparamref name="TInterface"/> interface.
        /// </summary>
        /// <typeparam name="TInterface">Interface</typeparam>
        /// <param name="type">Extends <see cref="Type"/>.</param>
        /// <param name="interface"></param>
        /// <example>
        /// <see href="https://gist.github.com/chrisstraw/212626e76c9eafedec24df4c2d106cea"/>
        /// </example>
        /// <returns>True if type implements interface</returns>
        public static bool Implements<TInterface>(this Type type, TInterface @interface) where TInterface : class
        {
            // ReSharper disable once PossibleNullReferenceException
            if (!(@interface as Type).IsInterface)
            {
                throw new InvalidOperationException("Only interfaces can be 'implemented'.");
            }

            return type.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Any(i => i.GetGenericTypeDefinition() == (@interface as Type));
        }

    }
}