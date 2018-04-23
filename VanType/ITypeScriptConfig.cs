namespace VanType
{
    public interface ITypeScriptConfig
    {
        ITypeScriptConfig Add<T>();
        string Generate();
        ITypeScriptConfig IncludeEnums();
        ITypeScriptConfig PrefixInterface();
    }
}