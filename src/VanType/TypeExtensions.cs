using System;
using System.Collections.Generic;
using System.Linq;

namespace VanType
{
    /// <summary>
    /// The type extensions.
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// Determines whether the type is a generic enumerable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is a generic enumerable; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsGenericEnumerable(this Type type)
        {
            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return type.GetGenericItemType() != null;
            }

            if (type
                .GetInterfaces()
                .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>)))
            {
                return type.GetGenericItemType() != null;
            }

            return false;
        }

        /// <summary>
        /// Gets the generic type of the enumerable.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The generic type of the enumerable.</returns>
        public static Type? GetGenericItemType(this Type type)
        {
            Type itemType = type.GetElementType();
            if (itemType != null)
            {
                return itemType;
            }
                
            var types = type.GetGenericArguments();
            if (types.Length == 1)
            {
                return types[0];
            }

            if (type.BaseType != null)
            {
                return type.BaseType.GetGenericItemType();
            }
            
            return null;
        }
    }
}