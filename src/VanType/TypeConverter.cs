using System;

namespace VanType
{
    public class TypeConverter
    {
        public TypeConverter(Type cSharpType, string typeScriptType, bool isNullable)
        {
            if (cSharpType == null)
            {
                throw new ArgumentNullException(nameof(cSharpType));
            }
            else if (typeScriptType == null)
            {
                throw new ArgumentNullException(nameof(typeScriptType));
            }
            else if (string.IsNullOrWhiteSpace(typeScriptType))
            {
                throw new ArgumentException("The supplied argument contains an empty string or whitespace.", nameof(typeScriptType));
            }

            CSharpType = cSharpType;
            TypeScriptType = typeScriptType;
            IsNullable = isNullable;
        }

        public Type CSharpType { get; }

        public bool IsNullable { get; set; }

        public string TypeScriptType { get; }

        public string GenerateType()
        {
            if (IsNullable)
            {
                return $"{TypeScriptType} | null";
            }

            return TypeScriptType;
        }

        public string GenerateArrayType()
        {
            return $"{TypeScriptType}[]";
        }
    }
}