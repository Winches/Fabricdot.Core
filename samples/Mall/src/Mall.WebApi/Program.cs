using Fabricdot.Core.Boot;
using Fabricdot.Infrastructure.DependencyInjection;
using Mall.Infrastructure.Data;
using Mall.WebApi;
using Serilog;
using Serilog.Events;

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
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.UseServiceProviderFactory(new FabricdotServiceProviderFactory());
    builder.Services.AddBootstrapper<MallApplicationModule>();
    var app = builder.Build();

    await app.BootstrapAsync();

    var dbMigrator = app.Services.GetRequiredService<DbMigrator>();
    await dbMigrator.MigrateAsync();

    await app.RunAsync();

    logger.Information("App host starting..");
}
catch (Exception ex)
{
    Log.Logger.Fatal(ex, "An error occurred when host running.");
}
finally
{
    logger.Information("App host shutting..");
}