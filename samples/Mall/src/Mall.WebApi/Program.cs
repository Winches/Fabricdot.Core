using System;
using System.IO;
using System.Threading.Tasks;
using Fabricdot.Infrastructure.DependencyInjection;
using Mall.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;

namespace Mall.WebApi;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var baseDir = AppDomain.CurrentDomain.BaseDirectory;
        var logfile = Path.Combine(baseDir, "logs", "app.log");
        Log.Logger = new LoggerConfiguration()
#if DEBUG
            .MinimumLevel.Debug()
#else
                .MinimumLevel.Information()
#endif
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
#if DEBUG
            .WriteTo.Async(c => c.Console())
#endif
            .WriteTo.Async(c => c.File(
                logfile,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Scope} {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day,
                rollOnFileSizeLimit: false))
            .CreateLogger();

        var logger = Log.Logger;
        try
        {
            var host = CreateHostBuilder(args).Build();

            var dbMigrator = host.Services.GetRequiredService<DbMigrator>();
            await dbMigrator.MigrateAsync();

            await host.RunAsync();

            logger.Information("App host starting..");
        }
        catch (Exception ex)
        {
            Log.Logger.Fatal(ex, "An error occurred when host running.");
        }
        finally
        {
            logger.Information("App host shutting..");
            Log.CloseAndFlush();
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
                   .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>())
                   .UseServiceProviderFactory(new FabricdotServiceProviderFactory());
    }
}