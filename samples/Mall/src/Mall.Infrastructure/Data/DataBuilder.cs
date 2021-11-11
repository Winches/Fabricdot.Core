using System.Threading.Tasks;

namespace Mall.Infrastructure.Data
{
    internal class DataBuilder
    {
        public Task SeedAsync()
        {
            //seed data
            return Task.CompletedTask;
        }
    }
}