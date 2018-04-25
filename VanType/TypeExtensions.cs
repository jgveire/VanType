using System;
using System.Collections.Generic;
using System.Linq;

namespace VanType
{
    public static class TypeExtensions
    {
        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterfaces().Any(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }
    }
}