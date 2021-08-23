﻿using System;
using Ardalis.GuardClauses;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

// ReSharper disable CheckNamespace

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static void AddEfDbContext<TDbContext>(
            this IServiceCollection serviceCollection,
            Action<DbContextOptionsBuilder> optionsAction = null) where TDbContext : DbContextBase
        {
            serviceCollection.AddEfDbContext<TDbContext>((_, builder) => optionsAction?.Invoke(builder));
        }

        public static void AddEfDbContext<TDbContext>(
            this IServiceCollection serviceCollection,
            Action<IServiceProvider, DbContextOptionsBuilder> optionsAction = null) where TDbContext : DbContextBase
        {
            Guard.Against.Null(serviceCollection, nameof(serviceCollection));

            serviceCollection.AddDbContext<TDbContext>((provider, opts) =>
            {
                optionsAction?.Invoke(provider, opts);
            });
            serviceCollection.AddScoped(typeof(IUnitOfWork<>), typeof(EfUnitOfWork<>));
            serviceCollection.AddTransient(typeof(IDbContextProvider<>), typeof(DefaultDbContextProvider<>));
        }
    }
}