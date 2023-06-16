using System.Collections.ObjectModel;

namespace Fabricdot.Core.DependencyInjection;

public class DependencyRegistrarCollection : Collection<IDependencyRegistrar>, IDependencyRegistrarCollection
{
}
