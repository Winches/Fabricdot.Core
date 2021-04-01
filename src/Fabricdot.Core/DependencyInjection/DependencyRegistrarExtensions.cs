using System;

namespace Fabricdot.Core.DependencyInjection
{
    public static class DependencyRegistrarExtensions
    {
        public static DependencyLifeTime GetDependencyLifeTime(this IDependencyRegistrar registrar, Type type)
        {
            if (type.IsAssignableTo(typeof(ITransientDependency)))
                return DependencyLifeTime.Transient;
            if (type.IsAssignableTo(typeof(ISingletonDependency)))
                return DependencyLifeTime.Singleton;
            if (type.IsAssignableTo(typeof(IScopedDependency)))
                return DependencyLifeTime.Scoped;

            return DependencyLifeTime.Transient;
        }
    }
}