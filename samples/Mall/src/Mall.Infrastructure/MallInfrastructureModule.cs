using Fabricdot.Core.Modularity;
using Fabricdot.Infrastructure;
using Fabricdot.Infrastructure.Data;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Mall.Domain;
using Mall.Infrastructure.Data;
using Mall.Infrastructure.Data.TypeHandlers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mall.Infrastructure;

[Requires(typeof(MallDomainModule))]
[Requires(typeof(FabricdotEntityFrameworkCoreModule))]
[Requires(typeof(FabricdotInfrastructureModule))]
[Exports]
public class MallInfrastructureModule : ModuleBase
{
    private static readonly ILoggerFactory _dbLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());

    public override void ConfigureServices(ConfigureServiceContext context)
    {
        var services = context.Services;

        #region database

        var connectionString = context.Configuration.GetConnectionString("Default");
        services.AddEfDbContext<AppDbContext>(opts =>
        {
            opts.UseSqlServer(connectionString);
#if DEBUG
            opts.UseLoggerFactory(_dbLoggerFactory)
                .EnableSensitiveDataLogging();
#endif
        });

        SqlMapperTypeHandlerConfiguration.AddTypeHandlers();
        services.AddScoped<ISqlConnectionFactory, DefaultSqlConnectionFactory>(_ => new DefaultSqlConnectionFactory(connectionString));

        #endregion database
    }
}