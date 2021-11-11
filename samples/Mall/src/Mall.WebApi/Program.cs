using System.Threading.Tasks;
using AspectCore.Extensions.DependencyInjection;
using Fabricdot.WebApi.Core;
using Mall.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Mall.WebApi
{
    public static class Program
    {
        public static async Task Main(string[] args)
        {
            await CreateHostBuilder(args).RunAsync(async host =>
            {
                var dbMigrator = host.Services.GetRequiredService<DbMigrator>();
                await dbMigrator.MigrateAsync();
            });
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var host = Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                .UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());
            host.AddModules();
            return host;
        }
    }
}