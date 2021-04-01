using System;

namespace Fabricdot.Core.DependencyInjection
{
    public interface IDependencyRegistrar
    {
        void Configure(object container, Type type);
    }
}