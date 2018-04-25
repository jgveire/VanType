using System;
using System.Reflection;

namespace VanType
{
    public interface ITypeScriptConfig
    {
        ITypeScriptConfig Add<TEntity>();
        ITypeScriptConfig AddAssembly<T>();
        ITypeScriptConfig AddTypeConverter<T>(string scriptType);
        string Generate();
        ITypeScriptConfig Import<TEntity>(string relativePath);
        ITypeScriptConfig IncludeEnums(bool value);
        ITypeScriptConfig OrderPropertiesByName(bool value);
        ITypeScriptConfig PrefixClasses(bool value);
        ITypeScriptConfig PrefixInterfaces(bool value);
    }
}