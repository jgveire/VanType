using System;

namespace VanType
{
    /// <summary>
    /// The type mapping between CSharp and TypeScript.
    /// </summary>
    public class TypeMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeMapping"/> class.
        /// </summary>
        /// <param name="cSharpType">The type in CSharp.</param>
        /// <param name="typeScriptType">The type in TypeScript.</param>
        /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
        /// <exception cref="ArgumentNullException">Thrown when cSharpType or typeScriptType is null.</exception>
        /// <exception cref="ArgumentException">Thrown when typeScriptType contains an empty string or whitespace.</exception>
        public TypeMapping(Type cSharpType, string typeScriptType, bool isNullable)
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

        /// <summary>
        /// Gets the CSharp type.
        /// </summary>
        /// <value>
        /// The the CSharp type.
        /// </value>
        public Type CSharpType { get; }

        /// <summary>
        /// Gets or sets a value indicating whether the CSharp type is nullable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the CSharp type is nullable; otherwise, <c>false</c>.
        /// </value>
        public bool IsNullable { get; set; }

        /// <summary>
        /// Gets the TypeScript type that should be used for the CSharp type.
        /// </summary>
        /// <value>
        /// The TypeScript type that should be used for the CSharp type.
        /// </value>
        public string TypeScriptType { get; set; }

        /// <summary>
        /// Generates the TypeScript type of an array.
        /// </summary>
        /// <returns>The TypeScript type for an array.</returns>
        public string GenerateArrayType()
        {
            return $"{TypeScriptType}[]";
        }

        /// <summary>
        /// Generates the TypeScript type.
        /// </summary>
        /// <returns>A TypeScript  type.</returns>
        public string GenerateType()
        {
            if (IsNullable)
            {
                return $"{TypeScriptType} | null";
            }

            return TypeScriptType;
        }
    }
}