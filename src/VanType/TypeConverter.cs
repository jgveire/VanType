using System;
using System.Collections.Generic;
using System.Linq;

namespace VanType
{
    /// <summary>
    /// The CSharp type to TypeScript type converter.
    /// </summary>
    public class TypeConverter
    {
        private readonly List<TypeMapping> _typeMappings = GetTypeMappings();

        /// <summary>
        /// Adds or replaces a type mapping.
        /// </summary>
        /// <param name="type">The CSharp type.</param>
        /// <param name="scriptType">The TypeScript type.</param>
        /// <param name="isNullable">Indicates weather the CSharp type is nullable or not.</param>
        public void AdddOrReplaceMapping(Type type, string scriptType, bool isNullable)
        {
            var mapping = _typeMappings.FirstOrDefault(c => c.CSharpType == type);
            if (mapping != null)
            {
                mapping.TypeScriptType = scriptType;
                mapping.IsNullable = isNullable;
            }
            else
            {
                _typeMappings.Add(new TypeMapping(type, scriptType, isNullable));
            }
        }

        /// <summary>
        /// Gets a mapping by CSharp type.
        /// </summary>
        /// <param name="type">The CSharp type.</param>
        /// <returns>A type mapping or null.</returns>
        public TypeMapping? GetMapping(Type type)
        {
            return _typeMappings.FirstOrDefault(c => c.CSharpType == type);
        }

        private static List<TypeMapping> GetTypeMappings()
        {
            return new List<TypeMapping>
            {
                new TypeMapping(typeof(string), "string", true),
                new TypeMapping(typeof(object), "object", true),
                new TypeMapping(typeof(DateTime), "Date", false),
                new TypeMapping(typeof(DateTime?), "Date", true),
                new TypeMapping(typeof(Guid), "string", false),
                new TypeMapping(typeof(Guid?), "string", true),
                new TypeMapping(typeof(bool), "boolean", false),
                new TypeMapping(typeof(bool?), "boolean", true),
                new TypeMapping(typeof(byte), "number", false),
                new TypeMapping(typeof(byte?), "number", true),
                new TypeMapping(typeof(sbyte), "number", false),
                new TypeMapping(typeof(sbyte?), "number", true),
                new TypeMapping(typeof(decimal), "number", false),
                new TypeMapping(typeof(decimal?), "number", true),
                new TypeMapping(typeof(double), "number", false),
                new TypeMapping(typeof(double?), "number", true),
                new TypeMapping(typeof(float), "number", false),
                new TypeMapping(typeof(float?), "number", true),
                new TypeMapping(typeof(int), "number", false),
                new TypeMapping(typeof(int?), "number", true),
                new TypeMapping(typeof(uint), "number", false),
                new TypeMapping(typeof(uint?), "number", true),
                new TypeMapping(typeof(int), "number", false),
                new TypeMapping(typeof(int?), "number", true),
                new TypeMapping(typeof(long), "number", false),
                new TypeMapping(typeof(long?), "number", true),
                new TypeMapping(typeof(ulong), "number", false),
                new TypeMapping(typeof(ulong?), "number", true),
                new TypeMapping(typeof(short), "number", false),
                new TypeMapping(typeof(short?), "number", true),
                new TypeMapping(typeof(ushort), "number", false),
                new TypeMapping(typeof(ushort?), "number", true),
            };
        }
    }
}
