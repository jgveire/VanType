using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VanType
{
    /// <summary>
    /// The TypeScript configuration.
    /// </summary>
    /// <seealso cref="VanType.ITypeScriptConfig" />
    public class TypeScript : ITypeScriptConfig
    {
        private readonly Dictionary<Type, string> _classImports = new Dictionary<Type, string>();
        private readonly List<Type> _enumTypes = new List<Type>();
        private readonly List<ClassProperty> _excludedProperties = new List<ClassProperty>();
        private readonly List<Type> _excludedTypes = new List<Type>();
        private readonly List<Type> _types = new List<Type>();
        private readonly TypeConverter _typeConverter = new TypeConverter();
        private bool _includeEnums = true;
        private bool _orderPropertiesByName = true;
        private bool _prefixClasses;
        private bool _prefixInterface;
        private bool _preserveInheritance;
        private Func<string, string>? _transformClassNameExpression;
        private Func<string, string>? _transformPropertyNameExpression;

        /// <summary>
        /// Creates a new TypeScript configurations.
        /// </summary>
        /// <returns></returns>
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
        public ITypeScriptConfig AddClass<TEntity>()
        {
            return Add(typeof(TEntity));
        }

        /// <inheritdoc />
        public ITypeScriptConfig AddType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            else  if (!_types.Contains(type))
            {
                _types.Add(type);
            }

            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig AddTypeConverter<T>(string scriptType, bool isNullable)
        {
            _typeConverter.AdddOrReplaceMapping(typeof(T), scriptType, isNullable);
            return this;
        }

        /// <inheritdoc />
        public ITypeScriptConfig ExcludeClass<T>()
        {
            if (!_excludedTypes.Contains(typeof(T)))
            {
                _excludedTypes.Add(typeof(T));
            }

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
        public string Generate()
        {
            StringBuilder script = new StringBuilder();

            GenerateImports(script);
            GenerateInterfaces(script);
            GenerateEnums(script);

            return script.ToString();
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

        public ITypeScriptConfig TransformPropertyName(Func<string, string> expression)
        {

            _transformPropertyNameExpression = expression;
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

        private void GenerateClassImport(Type type, string relativePath, StringBuilder script)
        {
            string name = GetInterfaceName(type);
            script.AppendLine($"import {{ {name} }} from '{relativePath}';");
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
            foreach (var value in Enum.GetValues(type))
            {
                string name = value.ToString();
                script.AppendLine($"\t{name} = {(int)value},");
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
            GenerateProperties(type, script);
            script.AppendLine("}");
        }

        private void GenerateInterfaces(StringBuilder script)
        {
            foreach (Type type in _types)
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

        private void GenerateProperties(Type type, StringBuilder script)
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

                string name = GetPropertyName(property);
                string typeName = GetPropertyType(property.PropertyType);
                script.AppendLine($"\t{name}: {typeName};");
            }
        }

        private string? GetBaseName(Type type)
        {
            if (type.BaseType != null && type.BaseType != typeof(object))
            {
                return GetInterfaceName(type.BaseType);
            }

            return null;
        }

        private string GetEnumName(Type type)
        {
            return type.Name;
        }

        private string GetInterfaceName(Type type)
        {
            string name = $"I{type.Name}";
            if (type.IsGenericTypeDefinition)
            {
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
            }
            else if (type.IsGenericParameter)
            {
                name = type.Name;
            }
            else if (type.IsInterface && !_prefixInterface)
            {
                name = type.Name;
            }
            else if (!type.IsInterface && !_prefixClasses)
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

        private string GetPropertyType(Type propertyType)
        {
            string typeName = GetTypeScriptType(propertyType);
            if (!typeName.Contains(" | null") &&
                !typeName.Contains("[]") &&
                (propertyType.IsClass || propertyType.IsInterface))
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

            if (type.IsEnum)
            {
                return type.Name;
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

        private bool ShouldExcludeType(Type type)
        {
            return type.IsNested ||
                (type.IsGenericType && !type.IsGenericTypeDefinition) ||
                _excludedTypes.Contains(type);
        }
    }
}
