using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VanType
{
    public class TypeScript : ITypeScriptConfig
    {
        private readonly Dictionary<Type, string> _classImports = new Dictionary<Type, string>();
        private readonly List<Type> _enumTypes = new List<Type>();
        private readonly List<TypeConverter> _typeConverters = GetConverters();
        private readonly List<Type> _types = new List<Type>();
        private bool _includeEnums = true;
        private bool _orderPropertiesByName = true;
        private bool _prefixClasses;
        private bool _prefixInterface;

        public ITypeScriptConfig AddClass<TEntity>()
        {
            return Add(typeof(TEntity));
        }

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

        public ITypeScriptConfig AddTypeConverter<T>(string scriptType, bool isNullable)
        {
            var converter = _typeConverters.FirstOrDefault(c => c.CSharpType == typeof(T));
            if (converter != null)
            {
                _typeConverters.Remove(converter);
            }

            _typeConverters.Add(new TypeConverter(typeof(T), scriptType, isNullable));
            return this;
        }

        public string Generate()
        {
            StringBuilder script = new StringBuilder();

            GenerateImports(script);
            GenerateInterfaces(script);
            GenerateEnums(script);

            return script.ToString();
        }

        public ITypeScriptConfig Import<TEntity>(string relativePath)
        {
            if (!_classImports.ContainsKey(typeof(TEntity)))
            {
                _classImports.Add(typeof(TEntity), relativePath);
            }

            return this;
        }

        public ITypeScriptConfig IncludeEnums(bool value)
        {
            _includeEnums = value;
            return this;
        }

        public ITypeScriptConfig OrderPropertiesByName(bool value)
        {
            _orderPropertiesByName = value;
            return this;
        }


        public ITypeScriptConfig PrefixClasses(bool value)
        {
            _prefixClasses = value;
            return this;
        }

        public ITypeScriptConfig PrefixInterfaces(bool value)
        {
            _prefixInterface = value;
            return this;
        }

        public static ITypeScriptConfig Config()
        {
            return new TypeScript();
        }

        public ITypeScriptConfig Add(Type type)
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

        public ITypeScriptConfig ExcludeEnums()
        {
            _includeEnums = false;
            return this;
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
            else if (type.IsEnumerable())
            {
                Type itemType = type.GetGenericArguments()[0];

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
            else if(type.IsInterface)
            {
                return type.Name;
            }

            return "any";
        }

        private TypeConverter GetTypeConverter(Type type)
        {
            return _typeConverters.FirstOrDefault(c => c.CSharpType == type);
            
        }

        private static List<TypeConverter> GetConverters()
        {
            return new List<TypeConverter>
            {
                new TypeConverter(typeof(DateTime), "Date", false),
                new TypeConverter(typeof(DateTime), "Date", true),
                new TypeConverter(typeof(string), "string", true),
                new TypeConverter(typeof(Guid), "string", false),
                new TypeConverter(typeof(Guid?), "string", true),
                new TypeConverter(typeof(bool), "boolean", false),
                new TypeConverter(typeof(bool?), "boolean", true),
                new TypeConverter(typeof(object), "object", true),
                new TypeConverter(typeof(byte), "number", false),
                new TypeConverter(typeof(byte?), "number", true),
                new TypeConverter(typeof(sbyte), "number", false),
                new TypeConverter(typeof(sbyte?), "number", true),
                new TypeConverter(typeof(decimal), "number", false),
                new TypeConverter(typeof(decimal?), "number", true),
                new TypeConverter(typeof(double), "number", false),
                new TypeConverter(typeof(double?), "number", true),
                new TypeConverter(typeof(float), "number", false),
                new TypeConverter(typeof(float?), "number", true),
                new TypeConverter(typeof(int), "number", false),
                new TypeConverter(typeof(int?), "number", true),
                new TypeConverter(typeof(uint), "number", false),
                new TypeConverter(typeof(uint?), "number", true),
                new TypeConverter(typeof(int), "number", false),
                new TypeConverter(typeof(int?), "number", true),
                new TypeConverter(typeof(long), "number", false),
                new TypeConverter(typeof(long?), "number", true),
                new TypeConverter(typeof(ulong), "number", false),
                new TypeConverter(typeof(ulong?), "number", true),
                new TypeConverter(typeof(short), "number", false),
                new TypeConverter(typeof(short?), "number", true),
                new TypeConverter(typeof(ushort), "number", false),
                new TypeConverter(typeof(ushort?), "number", true),
            };
        }

        private void AddEnum(Type type)
        {
            if (!_enumTypes.Contains(type))
            {
                _enumTypes.Add(type);
            }
        }

        private void AddType(Type type)
        {
            if (!_types.Contains(type))
            {
                _types.Add(type);
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
            script.AppendLine($"import {{ {name} }} from '{relativePath}'");
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
            string name = GetInterfaceName(type);
            script.AppendLine($"export interface {name}");
            script.AppendLine("{");
            GenerateProperties(type, script);
            script.AppendLine("}");
        }

        private void GenerateInterfaces(StringBuilder script)
        {
            foreach (Type type in _types)
            {
                if (type.IsEnum)
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
                if (CanAddToEnumCollection(property))
                {
                    _enumTypes.Add(property.PropertyType);
                }

                string name = GetPropertyName(property);
                string typeName = GetTypeScriptType(property.PropertyType);
                script.AppendLine($"\t{name}: {typeName};");
            }
        }

        private string GetEnumName(Type type)
        {
            return type.Name;
        }

        private string GetInterfaceName(Type type)
        {
            if (type.IsInterface && !_prefixInterface)
            {
                return type.Name;
            }
            else if (!type.IsInterface && !_prefixClasses)
            {
                return type.Name;
            }

            return $"I{type.Name}";
        }

        private IEnumerable<PropertyInfo> GetProperties(Type type)
        {
            IEnumerable<PropertyInfo> properties = type
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);
            if (_orderPropertiesByName)
            {
                properties = properties.OrderBy(p => p.Name);
            }

            return properties;
        }

        private string GetPropertyName(PropertyInfo property)
        {
            string name = property.Name;

            if (name.Length == 1)
            {
                return name[0].ToString().ToLower();
            }

            return $"{name[0].ToString().ToLower()}{property.Name.Substring(1)}";
        }
    }
}
