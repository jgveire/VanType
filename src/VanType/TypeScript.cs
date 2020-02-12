namespace VanType
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using Namotion.Reflection;

    /// <summary>
    /// The TypeScript configuration.
    /// </summary>
    /// <seealso cref="VanType.ITypeScriptConfig" />
    public class TypeScript : ITypeScriptConfig
    {
        private readonly Dictionary<Type, string> _classImports = new Dictionary<Type, string>();
        private readonly List<Type> _enumTypes = new List<Type>();
        private readonly List<ClassProperty> _excludedProperties = new List<ClassProperty>();
        private readonly List<string> _excludedTypeNames = new List<string>();
        private readonly List<Type> _excludedTypes = new List<Type>();
        private readonly TypeConverter _typeConverter = new TypeConverter();
        private readonly List<Type> _types = new List<Type>();
        private EnumConversionType _enumConversionType = EnumConversionType.Numeric;
        private bool _includeEnums = true;
        private bool _makeAllPropertiesNullable = true;
        private bool _orderPropertiesByName = true;
        private bool _prefixClasses;
        private bool _prefixInterface;
        private bool _preserveInheritance;
        private Func<string, string>? _transformClassNameExpression;
        private Func<string, string>? _transformPropertyNameExpression;

        /// <summary>
        /// Creates a new TypeScript configurations.
        /// </summary>
        /// <returns>The TypeScript configuration.</returns>
        public static ITypeScriptConfig Config()
        {
            return new TypeScript();
        }

        /// <inheritdoc />
        public ITypeScriptConfig AddAssembly<T>()
        {
            var types = typeof(T)
                .Assembly
                .GetTypes()
                .Where(type => type.IsClass && !type.IsAbstract);
            foreach (Type type in types)
            {
                Add(type);
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig AddType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            else if (!_types.Contains(type))
            {
                _types.Add(type);
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig AddType<T>()
        {
            return AddType(typeof(T));
        }

        /// <inheritdoc />
        public ITypeScriptConfig AddTypeConverter<T>(string scriptType, string defaultValue, bool isNullable)
        {
            _typeConverter.AdddOrReplaceMapping(typeof(T), scriptType, defaultValue, isNullable);
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig ExcludeProperty<T>(string propertyName)
        {
            var property = new ClassProperty(typeof(T), propertyName);
            if (!_excludedProperties.Contains(property))
            {
                _excludedProperties.Add(property);
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig ExcludeType<T>()
        {
            return ExcludeType(typeof(T));
        }

        /// <inheritdoc />
        public ITypeScriptConfig ExcludeType(Type type)
        {
            if (!_excludedTypes.Contains(type))
            {
                _excludedTypes.Add(type);
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig ExcludeType(string typeName)
        {
            if (!_excludedTypeNames.Contains(typeName))
            {
                _excludedTypeNames.Add(typeName);
            }

            return this;
        }

        /// <inheritdoc />
        public string GenerateClasses()
        {
            StringBuilder script = new StringBuilder();

            GenerateImports(script);
            GenerateClasses(script);
            GenerateEnums(script);

            return script.ToString();
        }

        /// <inheritdoc />
        public string GenerateInterfaces()
        {
            StringBuilder script = new StringBuilder();

            GenerateImports(script);
            GenerateInterfaces(script);
            GenerateEnums(script);

            return script.ToString();
        }

        /// <summary>
        /// Gets the interface name for the supplied type.
        /// </summary>
        /// <param name="type">The type to retrieve the interface name from.</param>
        /// <returns>A TypeScript interface name.</returns>
        public string GetInterfaceName(Type type)
        {
            string name = $"I{type.Name}";
            if (type.IsGenericTypeDefinition)
            {
                name = GetGenericTypeDefinitionName(type);
            }
            else if (type.IsGenericType)
            {
                name = GetGenericTypeName(type);
            }
            else if (type.IsGenericParameter ||
                (type.IsInterface && !_prefixInterface) ||
                (!type.IsInterface && !_prefixClasses))
            {
                name = type.Name;
            }

            if (_transformClassNameExpression != null)
            {
                name = _transformClassNameExpression(name);
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new InvalidOperationException("The interface name cannot be null or empty.");
                }
            }

            return name;
        }

        /// <inheritdoc />
        public ITypeScriptConfig Import<TEntity>(string relativePath)
        {
            if (!_classImports.ContainsKey(typeof(TEntity)))
            {
                _classImports.Add(typeof(TEntity), relativePath);
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig IncludeEnums(bool value)
        {
            _includeEnums = value;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig MakeAllPropertiesNullable(bool value)
        {
            _makeAllPropertiesNullable = value;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig OrderPropertiesByName(bool value)
        {
            _orderPropertiesByName = value;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig PrefixClasses(bool value)
        {
            _prefixClasses = value;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig PrefixInterfaces(bool value)
        {
            _prefixInterface = value;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig PreserveInheritance(bool value)
        {
            _preserveInheritance = value;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig TransformClassName(Func<string, string> expression)
        {
            _transformClassNameExpression = expression;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig TransformPropertyName(Func<string, string> expression)
        {
            _transformPropertyNameExpression = expression;
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig UseEnumConversion(EnumConversionType conversionType)
        {
            _enumConversionType = conversionType;
            return this;
        }

        private ITypeScriptConfig Add(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            else if (type.IsEnum)
            {
                AddEnum(type);
            }
            else
            {
                AddType(type);
            }

            return this;
        }

        private void AddEnum(Type type)
        {
            if (!_enumTypes.Contains(type))
            {
                _enumTypes.Add(type);
            }
        }

        private bool CanAddToEnumCollection(PropertyInfo property)
        {
            return property.PropertyType.IsEnum &&
                   _includeEnums &&
                   !_enumTypes.Contains(property.PropertyType);
        }

        private void GenerateClass(Type type, StringBuilder script)
        {
            string? baseName = null;
            var name = GetInterfaceName(type);
            script.Append($"export class {name}");
            if (_preserveInheritance)
            {
                baseName = GetBaseName(type);
                if (!string.IsNullOrEmpty(baseName))
                {
                    script.Append($" extends {baseName}");
                }
            }

            script.AppendLine();
            script.AppendLine("{");
            script.AppendLine($"    constructor(init?: Partial<{name}>) {{");
            if (!string.IsNullOrEmpty(baseName))
            {
                script.AppendLine("    super();");
            }

            script.AppendLine("        Object.assign(this, init);");
            script.AppendLine("    }");
            GenerateClassProperties(type, script);
            script.AppendLine("}");
        }

        private void GenerateClasses(StringBuilder script)
        {
            var types = _types
                .OrderBy(e => e.GetInheritanceCount())
                .ThenBy(e => e.Name);
            foreach (Type type in types)
            {
                if (ShouldExcludeType(type))
                {
                    continue;
                }
                else if (type.IsEnum)
                {
                    GenerateEnum(type, script);
                }
                else
                {
                    GenerateClass(type, script);
                }

                script.AppendLine(string.Empty);
            }
        }

        private void GenerateClassImport(Type type, string relativePath, StringBuilder script)
        {
            string name = GetInterfaceName(type);
            script.AppendLine($"import {{ {name} }} from '{relativePath}';");
        }

        private void GenerateClassProperties(Type type, StringBuilder script)
        {
            GenerateProperties(type, script, true);
        }

        private void GenerateEnum(Type type, StringBuilder script)
        {
            string name = GetEnumName(type);
            script.AppendLine($"export enum {name}");
            script.AppendLine("{");
            GenerateEnumValues(type, script);
            script.AppendLine("}");
        }

        private void GenerateEnums(StringBuilder script)
        {
            foreach (Type type in _enumTypes)
            {
                if (_excludedTypes.Contains(type))
                {
                    continue;
                }

                GenerateEnum(type, script);
                script.AppendLine(string.Empty);
            }
        }

        private void GenerateEnumValues(Type type, StringBuilder script)
        {
            var values = new List<string>();
            foreach (var value in Enum.GetValues(type))
            {
                var name = value.ToString();
                if (values.Contains(name))
                {
                    continue;
                }

                values.Add(name);
                if (_enumConversionType == EnumConversionType.Numeric)
                {
                    script.AppendLine($"    {name} = {(int)value},");
                }
                else if (_enumConversionType == EnumConversionType.String)
                {
                    script.AppendLine($"    {name} = '{value}',");
                }
                else
                {
                    throw new InvalidOperationException($"Unsupported EnumConversionType: {_enumConversionType}");
                }
            }
        }

        private void GenerateImports(StringBuilder script)
        {
            foreach (var classImport in _classImports)
            {
                GenerateClassImport(classImport.Key, classImport.Value, script);
            }

            if (_classImports.Any())
            {
                script.AppendLine(string.Empty);
            }
        }

        private void GenerateInterface(Type type, StringBuilder script)
        {
            var name = GetInterfaceName(type);
            script.Append($"export interface {name}");
            if (_preserveInheritance)
            {
                var baseName = GetBaseName(type);
                if (!string.IsNullOrEmpty(baseName))
                {
                    script.Append($" extends {baseName}");
                }
            }

            script.AppendLine();
            script.AppendLine("{");
            GenerateInterfaceProperties(type, script);
            script.AppendLine("}");
        }

        private void GenerateInterfaceProperties(Type type, StringBuilder script)
        {
            GenerateProperties(type, script, false);
        }

        private void GenerateInterfaces(StringBuilder script)
        {
            var types = _types
                .OrderBy(e => e.GetInheritanceCount())
                .ThenBy(e => e.Name);
            foreach (Type type in types)
            {
                if (ShouldExcludeType(type))
                {
                    continue;
                }
                else if (type.IsEnum)
                {
                    GenerateEnum(type, script);
                }
                else
                {
                    GenerateInterface(type, script);
                }

                script.AppendLine(string.Empty);
            }
        }

        private void GenerateProperties(Type type, StringBuilder script, bool forClass)
        {
            var properties = GetProperties(type);
            foreach (PropertyInfo property in properties)
            {
                var classProperty = new ClassProperty(type, property.Name);
                if (_excludedProperties.Contains(classProperty))
                {
                    continue;
                }

                if (CanAddToEnumCollection(property))
                {
                    _enumTypes.Add(property.PropertyType);
                }

                var generatedProperty = GenerateProperty(property, forClass);
                script.AppendLine(generatedProperty);
            }
        }

        private string GenerateProperty(PropertyInfo property, bool withDefaultValue)
        {
            string name = GetPropertyName(property);
            string typeName = GetPropertyType(property);
            if (withDefaultValue)
            {
                string defaultValue = GetDefaultPropertyValue(property.PropertyType);
                return $"    {name}: {typeName} = {defaultValue};";
            }

            return $"    {name}: {typeName};";
        }

        private string? GetBaseName(Type type)
        {
            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                return GetInterfaceName(type.BaseType);
            }

            return null;
        }

        private string GetDefaultPropertyValue(Type propertyType)
        {
            var converter = GetTypeConverter(propertyType);
            if (converter != null)
            {
                return converter.DefaultValue;
            }

            if (propertyType.IsEnum)
            {
                string name = GetEnumName(propertyType);
                object value = Enum.GetValues(propertyType).GetValue(0);
                return $"{name}.{value}";
            }
            else if (propertyType.IsGenericEnumerable())
            {
                return "[]";
            }

            return "null";
        }

        private string GetEnumName(Type type)
        {
            return type.Name;
        }

        private string GetGenericTypeDefinitionName(Type type)
        {
            string name;
            var types = string.Join(", ", type.GetGenericArguments().Select(e => e.Name));
            var index = type.Name.IndexOf("`");
            var tempName = type.Name.Substring(0, index);
            if ((type.IsInterface && _prefixInterface) ||
                (!type.IsInterface && _prefixClasses))
            {
                name = $"I{tempName}<{types}>";
            }
            else
            {
                name = $"{tempName}<{types}>";
            }

            return name;
        }

        private string GetGenericTypeName(Type type)
        {
            string name;
            var types = string.Join(", ", type.GetGenericArguments().Select(e => GetTypeScriptType(e)));
            var index = type.Name.IndexOf("`");
            var tempName = type.Name.Substring(0, index);
            if ((type.IsInterface && _prefixInterface) ||
                (!type.IsInterface && _prefixClasses))
            {
                name = $"I{tempName}<{types}>";
            }
            else
            {
                name = $"{tempName}<{types}>";
            }

            return name;
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            IEnumerable<PropertyInfo> properties = _preserveInheritance ?
                type.GetProperties(BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance | BindingFlags.DeclaredOnly) :
                type.GetProperties(BindingFlags.Public | BindingFlags.GetField | BindingFlags.Instance);

            if (_orderPropertiesByName)
            {
                return properties.OrderBy(p => p.Name);
            }

            return properties;
        }

        private string GetPropertyName(PropertyInfo property)
        {
            string name;
            if (property.Name.Length == 1)
            {
                name = property.Name[0].ToString().ToLower();
            }
            else
            {
                name = $"{property.Name[0].ToString().ToLower()}{property.Name.Substring(1)}";
            }

            if (_transformPropertyNameExpression != null)
            {
                name = _transformPropertyNameExpression(name);
                if (string.IsNullOrWhiteSpace(name))
                {
                    throw new InvalidOperationException("The property name cannot be null or empty.");
                }
            }

            return name;
        }

        private string GetPropertyType(PropertyInfo property)
        {
            string typeName = GetTypeScriptType(property.PropertyType);
            if ((IsTypeNullable(property) || _makeAllPropertiesNullable) &&
                !typeName.Contains(" | null") &&
                !typeName.Contains("[]"))
            {
                return $"{typeName} | null";
            }

            return typeName;
        }

        private TypeMapping? GetTypeConverter(Type type)
        {
            return _typeConverter.GetMapping(type);
        }

        private string GetTypeScriptType(Type type)
        {
            var converter = GetTypeConverter(type);
            if (converter != null)
            {
                return converter.GenerateType();
            }
            else if (type.IsEnum)
            {
                return type.Name;
            }
            else if (Nullable.GetUnderlyingType(type)?.IsEnum == true)
            {
                return Nullable.GetUnderlyingType(type).Name;
            }
            else if (type.IsGenericEnumerable())
            {
                Type? itemType = type.GetGenericItemType();
                if (itemType == null)
                {
                    return $"any[]";
                }

                converter = GetTypeConverter(itemType);
                if (converter != null)
                {
                    return converter.GenerateArrayType();
                }

                string typeScriptType = GetTypeScriptType(itemType);
                return $"{typeScriptType}[]";
            }
            else if (type.IsClass)
            {
                return GetInterfaceName(type);
            }
            else if (type.IsInterface)
            {
                return type.Name;
            }

            return "any";
        }

        private bool IsTypeNullable(PropertyInfo property)
        {
            var converter = GetTypeConverter(property.PropertyType);
            if (converter != null)
            {
                return converter.IsNullable;
            }

            return property.ToContextualProperty().IsNullableType;
        }

        private bool ShouldExcludeType(Type type)
        {
            return type.IsNested ||
                (type.IsGenericType && !type.IsGenericTypeDefinition) ||
                _excludedTypes.Contains(type) ||
                _excludedTypeNames.Contains(type.Name);
        }
    }
}
