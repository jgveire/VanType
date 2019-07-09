namespace VanType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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

        /// <summary>
        /// Gets the number of classes the type inherits.
        /// </summary>
        /// <param name="type">The type to retrieve the inheritance count for.</param>
        /// <returns>The number of classes the type inherits.</returns>
        public static int GetInheritanceCount(this Type type)
        {
            var count = 0;
            while (type != null)
            {
                count++;
                type = type.BaseType;
            }

            return count;
        }
    }
}