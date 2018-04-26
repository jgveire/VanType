using System;
using System.Collections.Generic;
using System.Linq;

namespace VanType
{
    public static class TypeExtensions
    {
        public static bool IsEnumerable(this Type type)
        {
            if (type.IsInterface && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(IEnumerable<>))
            {
                return true;
            }

            return type
                .GetInterfaces()
                .Any(t => t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}