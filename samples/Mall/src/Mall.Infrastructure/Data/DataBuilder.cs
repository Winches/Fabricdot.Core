using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;

namespace Mall.Infrastructure.Data;

internal class DataBuilder : ITransientDependency
{
    public Task SeedAsync()
    {
        //seed data
        return Task.CompletedTask;
    }
}