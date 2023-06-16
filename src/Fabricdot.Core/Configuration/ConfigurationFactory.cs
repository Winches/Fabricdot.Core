using Microsoft.Extensions.Configuration;

namespace Fabricdot.Core.Configuration;

public static class ConfigurationFactory
{
    public static IConfigurationRoot Build(
        ConfigurationBuilderOptions? options = null,
        Action<IConfigurationBuilder>? builderAction = null)
    {
        options ??= new ConfigurationBuilderOptions();
        var basePath = options.BasePath.IsNullOrEmpty()
            ? Directory.GetCurrentDirectory()
            : options.BasePath;

        var builder = new ConfigurationBuilder().SetBasePath(basePath)
                                                .AddJsonFile($"{options.FileName}.json", optional: true, reloadOnChange: true)
                                                .AddEnvironmentVariables(options.EnvironmentVariablesPrefix);

        if (!options.EnvironmentName.IsNullOrEmpty())
        {
            builder = builder.AddJsonFile(
                $"{options.FileName}.{options.EnvironmentName}.json",
                optional: true,
                reloadOnChange: true);
        }

        if (options.EnvironmentName == "Development")
        {
            if (options.UserSecretsId != null)
            {
                builder.AddUserSecrets(options.UserSecretsId);
            }
            else if (options.UserSecretsAssembly != null)
            {
                builder.AddUserSecrets(options.UserSecretsAssembly, true);
            }
        }

        if (options.CommandLineArgs != null)
        {
            builder = builder.AddCommandLine(options.CommandLineArgs);
        }

        builderAction?.Invoke(builder);

        return builder.Build();
    }
}
