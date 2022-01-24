﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Fabricdot.MultiTenancy.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Fabricdot.MultiTenancy
{
    public class TenantResolver : ITenantResolver
    {
        private readonly MultiTenancyOptions _options;
        private readonly IServiceProvider _serviceProvider;

        public TenantResolver(
            IOptions<MultiTenancyOptions> options,
            IServiceProvider serviceProvider)
        {
            _options = options.Value;
            _serviceProvider = serviceProvider;
        }

        public async Task<TenantResolveResult> ResolveAsync()
        {
            var ret = new TenantResolveResult();
            var context = new TenantResolveContext(_serviceProvider);
            foreach (var strategy in _options.ResolveStrategies.OrderBy(v => v.Priority))
            {
                var identifier = await DecorateStrategy(strategy).ResolveIdentifierAsync(context);
                ret.Strategies.Add(strategy.GetType().FullName);

                if (!string.IsNullOrEmpty(identifier))
                {
                    ret.Identifier = identifier;
                    break;
                }
            }
            return ret;
        }

        private ITenantResolveStrategy DecorateStrategy(ITenantResolveStrategy strategy)
        {
            var loggerFactory = _serviceProvider.GetRequiredService<ILoggerFactory>();
            return new TenantResolveStrategyDecorator(
                loggerFactory.CreateLogger(strategy.GetType()),
                strategy);
        }
    }
}