using System;

namespace VanType
{
    public class TypeConverter
    {
        public TypeConverter(Type cSharpType, string typeScriptType)
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
        }

        public Type CSharpType { get; }

        public string TypeScriptType { get; }
    }
}