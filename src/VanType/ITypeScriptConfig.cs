﻿namespace VanType
{
    using System;

    /// <summary>
    /// The TypeScript configuration.
    /// </summary>
    public interface ITypeScriptConfig
    {
        /// <summary>
        /// Adds all models in a assembly to the configuration.
        /// </summary>
        /// <typeparam name="T">The type that should be used to determine the assembly.</typeparam>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig AddAssembly<T>();

        /// <summary>
        /// Adds a type to the configuration.
        /// </summary>
        /// <typeparam name="T">The type to add.</typeparam>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig AddType<T>();

        /// <summary>
        /// Adds a type to the configuration.
        /// </summary>
        /// <param name="type">The type to add.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig AddType(Type type);

        /// <summary>
        /// Adds a type converter to the configuration.
        /// </summary>
        /// <typeparam name="T">The type for which the type should be converted.</typeparam>
        /// <param name="scriptType">The type that should be used in TypeScript..</param>
        /// <param name="defaultValue">The default TypeScript value.</param>
        /// <param name="isNullable">if set to <c>true</c> the type is nullable.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig AddTypeConverter<T>(string scriptType, string defaultValue, bool isNullable);

        /// <summary>
        /// Excludes a class from generation.
        /// </summary>
        /// <typeparam name="T">The type that should be excluded from generation.</typeparam>
        /// <param name="propertyName">The name of the property.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig ExcludeProperty<T>(string propertyName);

        /// <summary>
        /// Excludes a type from generation.
        /// </summary>
        /// <typeparam name="T">The type that should be excluded from generation.</typeparam>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig ExcludeType<T>();

        /// <summary>
        /// Excludes a type from generation.
        /// </summary>
        /// <param name="type">The type that should be excluded from generation.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig ExcludeType(Type type);

        /// <summary>
        /// Excludes a type from generation.
        /// </summary>
        /// <param name="typeName">The name of the type that should be excluded from generation.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig ExcludeType(string typeName);

        /// <summary>
        /// Generates the TypeScript.
        /// </summary>
        /// <returns>A TypeScript file.</returns>
        string GenerateClasses();

        /// <summary>
        /// Generates the TypeScript definitions.
        /// </summary>
        /// <returns>A TypeScript definition file.</returns>
        string GenerateInterfaces();

        /// <summary>
        /// Imports the supplied type via the specified path.
        /// </summary>
        /// <typeparam name="T">The type that should be imported.</typeparam>
        /// <param name="relativePath">The relative path to the TypeScript definition file.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig Import<T>(string relativePath);

        /// <summary>
        /// Configures whether enumerations should be included automatically.
        /// </summary>
        /// <param name="value">if set to <c>true</c> enumerations will be included automatically.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig IncludeEnums(bool value);

        /// <summary>
        /// Configures whether all properties should be nullable.
        /// </summary>
        /// <param name="value">if set to <c>true</c> all properties will be nullable.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig MakeAllPropertiesNullable(bool value);

        /// <summary>
        /// Configures whether properties should be ordered alphabetically.
        /// </summary>
        /// <param name="value">if set to <c>true</c> properties will be ordered alphabetically.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig OrderPropertiesByName(bool value);

        /// <summary>
        /// Configures whether classes should be prefixed with a capital 'I'.
        /// </summary>
        /// <param name="value">if set to <c>true</c> classes will be prefixed with a capital 'I'.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig PrefixClasses(bool value);

        /// <summary>
        /// Configures whether interfaces should be prefixed with a capital 'I'.
        /// </summary>
        /// <param name="value">if set to <c>true</c> interfaces will be prefixed with a capital 'I'.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig PrefixInterfaces(bool value);

        /// <summary>
        /// Configures whether the inheritance should be preserved during generation.
        /// </summary>
        /// <param name="value">if set to <c>true</c> the inheritance will be preserved.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig PreserveInheritance(bool value);

        /// <summary>
        /// Transforms the name of the class.
        /// </summary>
        /// <param name="expression">The transform function.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig TransformClassName(Func<string, string> expression);

        /// <summary>
        /// Transforms the name of the property.
        /// </summary>
        /// <param name="expression">The transform function.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig TransformPropertyName(Func<string, string> expression);

        /// <summary>
        /// Configures which enumerations conversion should be used.
        /// </summary>
        /// <param name="conversionType">The enumerations conversion type to use when generating the TypeScript.</param>
        /// <returns>A TypeScript definition file.</returns>
        ITypeScriptConfig UseEnumConversion(EnumConversionType conversionType);
    }
}