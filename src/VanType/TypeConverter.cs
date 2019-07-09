namespace VanType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

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
        /// <param name="defaultValue">The default TypeScript value.</param>
        /// <param name="isNullable">Indicates weather the CSharp type is nullable or not.</param>
        public void AdddOrReplaceMapping(Type type, string scriptType, string defaultValue, bool isNullable)
        {
            var mapping = _typeMappings.FirstOrDefault(c => c.CSharpType == type);
            if (mapping != null)
            {
                mapping.TypeScriptType = scriptType;
                mapping.IsNullable = isNullable;
                mapping.DefaultValue = defaultValue;
            }
            else
            {
                _typeMappings.Add(new TypeMapping(type, scriptType, defaultValue, isNullable));
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
                new TypeMapping(typeof(string), "string", "''", true),
                new TypeMapping(typeof(object), "object", "null", true),
                new TypeMapping(typeof(DateTime), "Date", "new Date()", false),
                new TypeMapping(typeof(DateTime?), "Date", "null", true),
                new TypeMapping(typeof(Guid), "string", "'00000000-0000-0000-0000-000000000000'", false),
                new TypeMapping(typeof(Guid?), "string", "null", true),
                new TypeMapping(typeof(bool), "boolean", "false", false),
                new TypeMapping(typeof(bool?), "boolean", "null", true),
                new TypeMapping(typeof(byte), "number", "0", false),
                new TypeMapping(typeof(byte?), "number", "null", true),
                new TypeMapping(typeof(sbyte), "number", "0", false),
                new TypeMapping(typeof(sbyte?), "number", "null", true),
                new TypeMapping(typeof(decimal), "number", "0", false),
                new TypeMapping(typeof(decimal?), "number", "null", true),
                new TypeMapping(typeof(double), "number", "0", false),
                new TypeMapping(typeof(double?), "number", "null", true),
                new TypeMapping(typeof(float), "number", "0", false),
                new TypeMapping(typeof(float?), "number", "null", true),
                new TypeMapping(typeof(int), "number", "0", false),
                new TypeMapping(typeof(int?), "number", "null", true),
                new TypeMapping(typeof(uint), "number", "0", false),
                new TypeMapping(typeof(uint?), "number", "null", true),
                new TypeMapping(typeof(int), "number", "0", false),
                new TypeMapping(typeof(int?), "number", "null", true),
                new TypeMapping(typeof(long), "number", "0", false),
                new TypeMapping(typeof(long?), "number", "null", true),
                new TypeMapping(typeof(ulong), "number", "0", false),
                new TypeMapping(typeof(ulong?), "number", "null", true),
                new TypeMapping(typeof(short), "number", "0", false),
                new TypeMapping(typeof(short?), "number", "null", true),
                new TypeMapping(typeof(ushort), "number", "0", false),
                new TypeMapping(typeof(ushort?), "number", "null", true),
            };
        }
    }
}
