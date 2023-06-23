namespace System.Reflection;

public static class AttributeExtensions
{
    public static bool IsDefined<T>(
        this Type type,
        bool inherit = false) where T : Attribute
    {
        return type.IsDefined(typeof(T), inherit);
    }

    public static bool IsDefined<T>(
        this MemberInfo memberInfo,
        bool inherit = false) where T : Attribute
    {
        return memberInfo.IsDefined(typeof(T), inherit);
    }

    public static bool IsDefined<T>(
        this ParameterInfo parameterInfo,
        bool inherit = false) where T : Attribute
    {
        return parameterInfo.IsDefined(typeof(T), inherit);
    }
}
