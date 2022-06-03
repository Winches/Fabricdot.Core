using System.Reflection;

namespace Fabricdot.Infrastructure.Uow
{
    public static class UnitOfWorkUtil
    {
        public static UnitOfWorkAttribute? GetUnitOfWorkAttribute(MethodInfo methodInfo)
        {
            return methodInfo.GetCustomAttribute<UnitOfWorkAttribute>(true)
                   ?? methodInfo.DeclaringType?.GetCustomAttribute<UnitOfWorkAttribute>(true);
        }
    }
}