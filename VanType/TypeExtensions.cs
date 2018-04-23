using System;
using System.Collections.Generic;
using System.Linq;

namespace VanType
{
    public static class TypeExtensions
    {
        public static bool IsBoolean(this Type type)
        {
            if (type == typeof(bool)|| type == typeof(bool?))
            {
                return true;
            }

            return false;
        }

        public static bool IsDate(this Type type)
        {
            if (type == typeof(DateTime) || type == typeof(DateTime?))
            {
                return true;
            }

            return false;
        }

        public static bool IsEnumerable(this Type type)
        {
            return type.GetInterfaces().Any(t => t.GetGenericTypeDefinition() == typeof(IEnumerable<>));
        }

        public static bool IsNumber(this Type type)
        {
            if (type == typeof(byte) || type == typeof(byte?))
            {
                return true;
            }
            else if (type == typeof(sbyte) || type == typeof(sbyte?))
            {
                return true;
            }
            else if (type == typeof(decimal) || type == typeof(decimal?))
            {
                return true;
            }
            else if (type == typeof(double) || type == typeof(double?))
            {
                return true;
            }
            else if (type == typeof(float) || type == typeof(float?))
            {
                return true;
            }
            else if (type == typeof(int) || type == typeof(int?))
            {
                return true;
            }
            else if (type == typeof(uint) || type == typeof(uint?))
            {
                return true;
            }
            else if (type == typeof(int) || type == typeof(int?))
            {
                return true;
            }
            else if (type == typeof(long) || type == typeof(long?))
            {
                return true;
            }
            else if (type == typeof(ulong) || type == typeof(ulong?))
            {
                return true;
            }
            else if (type == typeof(short) || type == typeof(short?))
            {
                return true;
            }
            else if (type == typeof(ushort) || type == typeof(ushort?))
            {
                return true;
            }

            return false;
        }

        public static bool IsObject(this Type type)
        {
            if (type == typeof(object))
            {
                return true;
            }

            return false;
        }

        public static bool IsString(this Type type)
        {
            if (type == typeof(string))
            {
                return true;
            }

            return false;
        }
    }
}