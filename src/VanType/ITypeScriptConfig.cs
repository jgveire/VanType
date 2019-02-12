namespace VanType
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
        /// Adds a class to the configuration.
        /// </summary>
        /// <typeparam name="TClass">The type of the class.</typeparam>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig AddClass<TClass>();

        /// <summary>
        /// Adds a type converter to the configuration.
        /// </summary>
        /// <typeparam name="T">The type for which the type should be converted.</typeparam>
        /// <param name="scriptType">The type that should be used in TypeScript..</param>
        /// <param name="isNullable">if set to <c>true</c> the type is nullable.</param>
        /// <returns>The TypeScript configuration.</returns>
        ITypeScriptConfig AddTypeConverter<T>(string scriptType, bool isNullable);

        /// <summary>
        /// Generates the TypeScript definitions.
        /// </summary>
        /// <returns>A TypeScript definition file.</returns>
        string Generate();

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
    }
}