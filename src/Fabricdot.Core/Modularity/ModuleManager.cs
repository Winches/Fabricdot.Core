using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using Fabricdot.Core.Boot;
using Fabricdot.Core.Reflection;
using Microsoft.Extensions.Logging;

namespace Fabricdot.Core.Modularity
{
    public class ModuleManager : IModuleManager
    {
        private readonly ILogger<ModuleManager> _logger;
        private readonly IModuleCollection _modules;

        public IReadOnlyCollection<IModuleMetadata> Modules => _modules.ToImmutableList();

        public ModuleManager(
            ILogger<ModuleManager> logger,
            IModuleCollection modules)
        {
            Guard.Against.NullOrEmpty(modules, nameof(modules));

            _logger = logger;
            _modules = modules;
        }

        public virtual async Task StartAsync(ApplicationStartingContext context)
        {
            _logger.LogInformation("Modules starting...");

            foreach (var module in _modules.Reverse())
            {
                try
                {
                    if (module.Instance is IApplicationStarting application)
                        await application.OnStartingAsync(context);
                }
                catch (Exception e)
                {
                    throw new ModularityException($"Module start failed: {module.Type.PrettyPrint()}", e);
                }
            }

            _logger.LogInformation("Modules started...");
        }

        public virtual async Task StopAsync(ApplicationStoppingContext context)
        {
            _logger.LogInformation("Modules stopping...");

            foreach (var module in _modules.Reverse())
            {
                try
                {
                    if (module.Instance is IApplicationStopping application)
                        await application.OnStoppingAsync(context);
                }
                catch (Exception e)
                {
                    throw new ModularityException($"Module stop failed: {module.Type.PrettyPrint()}", e);
                }
            }

            _logger.LogInformation("Modules stopped...");
        }
    }
}