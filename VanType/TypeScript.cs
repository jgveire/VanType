using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace VanType
{
    public class TypeScript : ITypeScriptConfig
    {
        List<Type> _enumTypes = new List<Type>();
        private bool _includeEnums;
        private bool _prefixInterface;
        List<Type> _types = new List<Type>();

        public ITypeScriptConfig Add<T>()
        {
            if (!_types.Contains(typeof(T)))
            {
                _types.Add(typeof(T));
            }

            return this;
        }

        public string Generate()
        {
            StringBuilder script = new StringBuilder();
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

            foreach (Type type in _enumTypes)
            {
                GenerateEnum(type, script);
                script.AppendLine(string.Empty);
            }

            return script.ToString();
        }

        public ITypeScriptConfig IncludeEnums()
        {
            _includeEnums = true;
            return this;
        }

        public ITypeScriptConfig PrefixInterface()
        {
            _prefixInterface = true;
            return this;
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
                return "bool";
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
                return $"I{type.Name}";
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
                script.AppendLine($"\t{name} = {(int)value};");
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
            var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.GetField);
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
