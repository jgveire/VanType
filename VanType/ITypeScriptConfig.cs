namespace VanType
{
    public interface ITypeScriptConfig
    {
        ITypeScriptConfig Add<TEntity>();
        string Generate();
        ITypeScriptConfig IncludeEnums(bool value);
        ITypeScriptConfig PrefixInterfaces(bool value);
        ITypeScriptConfig PrefixClasses(bool value);
        ITypeScriptConfig OrderPropertiesByName(bool value);
        ITypeScriptConfig Import<TEntity>(string relativePath);
    }
}