using System;
using Fabricdot.Domain.Core.SharedKernel;
using Fabricdot.Infrastructure.Core.Data;
using Fabricdot.Infrastructure.Core.DependencyInjection;
using Fabricdot.WebApi.Core.Configuration;
using Mall.Infrastructure.Data;
using Mall.Infrastructure.Data.TypeHandlers;
using Mall.WebApi.Configuration;
using Mall.WebApi.Queries.Orders;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Mall.WebApi
{
    public class MallApplicationModule : IModule
    {
        private static readonly ILoggerFactory _dbLoggerFactory = LoggerFactory.Create(builder => builder.AddConsole());
        private readonly IConfiguration _configuration;

        public MallApplicationModule(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        /// <inheritdoc />
        public void Configure(IServiceCollection services)
        {
            #region endpoint

            services.AddControllers(opts => opts.AddActionFilters())
                .ConfigureApiBehaviorOptions(opts =>
                {
                    opts.SuppressModelStateInvalidFilter = true;
                });

            #endregion endpoint

            #region database

            var connectionString = _configuration.GetConnectionString("Default");
            services.AddEfDbContext<AppDbContext>(opts =>
            {
                opts.UseSqlServer(connectionString);
#if DEBUG
                opts.UseLoggerFactory(_dbLoggerFactory)
                    .EnableSensitiveDataLogging();
#endif
            });

            SqlMapperTypeHandlerConfiguration.AddTypeHandlers();
            services.AddScoped<ISqlConnectionFactory, SqlConnectionFactory>(_ =>
                new SqlConnectionFactory(connectionString));

            #endregion database

            #region api-doc

            //swagger
            services.AddSwagger();

            #endregion api-doc

            SystemClock.Configure(DateTimeKind.Utc);
            services.AddSingleton<IContentTypeProvider, FileExtensionContentTypeProvider>();
            //add project services here.
            services.AddTransient<IOrderQueries, OrderQueries>();
        }
    }
}