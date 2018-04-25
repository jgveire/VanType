using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VanType
{
    public class TypeScript : ITypeScriptConfig
    {
        private readonly List<Type> _enumTypes = new List<Type>();
        private readonly List<Type> _types = new List<Type>();
        private readonly Dictionary<Type, string> _classImports = new Dictionary<Type, string>();
        private bool _includeEnums = true;
        private bool _orderPropertiesByName = true;
        private bool _prefixClasses;
        private bool _prefixInterface;

        public ITypeScriptConfig Add<TEntity>()
        {
            if (typeof(TEntity).IsEnum)
            {
                AddEnum<TEntity>();
            }
            else
            {
                AddType<TEntity>();
            }

            return this;
        }

        private void AddType<TEntity>()
        {
            if (!_types.Contains(typeof(TEntity)))
            {
                _types.Add(typeof(TEntity));
            }
        }

        private void AddEnum<TEntity>()
        {
            if (!_enumTypes.Contains(typeof(TEntity)))
            {
                _enumTypes.Add(typeof(TEntity));
            }
        }

        public string Generate()
        {
            StringBuilder script = new StringBuilder();

            GenerateImports(script);
            GenerateInterfaces(script);
            GenerateEnums(script);

            return script.ToString();
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

        private void GenerateClassImport(Type type, string relativePath, StringBuilder script)
        {
            string name = GetInterfaceName(type);
            script.AppendLine($"import {{ {name} }} from '{relativePath}'");
        }

        private void GenerateEnums(StringBuilder script)
        {
            foreach (Type type in _enumTypes)
            {
                GenerateEnum(type, script);
                script.AppendLine(string.Empty);
            }
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

        public ITypeScriptConfig Import<TEntity>(string relativePath)
        {
            if (!_classImports.ContainsKey(typeof(TEntity)))
            {
                _classImports.Add(typeof(TEntity), relativePath);
            }

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

        public ITypeScriptConfig ExcludeEnums()
        {
            _includeEnums = false;
            return this;
        }

        public string GetTypeScriptType(Type type)
        {
            if (type.IsBoolean())
            {
                return "boolean";
            }
            else if (type.IsNumber())
            {
                return "number";
            }
            else if (type.IsString())
            {
                return "string";
            }
            else if (type.IsDate())
            {
                return "date";
            }
            else if (type.IsObject())
            {
                return "object";
            }
            else if (type.IsEnum)
            {
                return type.Name;
            }
            else if (type.IsEnumerable())
            {
                Type itemType = type.GetGenericArguments()[0];
                string s = GetTypeScriptType(itemType);
                return $"{s}[]";
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

        public bool IsKnownType(Type type)
        {
            if (type.IsBoolean() ||
                type.IsNumber() || 
                type.IsString() ||
                type.IsDate() ||
                type.IsObject())
            {
                return true;
            }
            else if (type.IsEnum || type.IsClass || type.IsInterface)
            {
                return _types.Contains(type);
            }
            else if (type.IsEnumerable())
            {
                Type itemType = type.GetGenericArguments()[0];
                return _types.Contains(itemType);
            }

            return false;
        }

        private bool CanAddToEnumCollection(PropertyInfo property)
        {
            return property.PropertyType.IsEnum &&
                   _includeEnums &&
                   !_enumTypes.Contains(property.PropertyType);
        }

        private void GenerateEnum(Type type, StringBuilder script)
        {
            string name = GetEnumName(type);
            script.AppendLine($"export enum {name}");
            script.AppendLine("{");
            GenerateEnumValues(type, script);
            script.AppendLine("}");
        }

        private void GenerateEnumValues(Type type, StringBuilder script)
        {
            foreach (var value in Enum.GetValues(type))
            {
                string name = value.ToString();
                script.AppendLine($"\t{name} = {(int)value},");
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
