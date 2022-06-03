using System.Reflection;

namespace Fabricdot.Core.Configuration
{
    public class ConfigurationBuilderOptions
    {
        /// <summary>
        ///     Base path to read the configuration file indicated by <see cref="FileName" />.
        /// </summary>
        public string? BasePath { get; set; }

        /// <summary>
        ///     Default value: "appsettings".
        /// </summary>
        public string FileName { get; set; } = "appsettings";

        /// <summary>
        ///     Command line arguments.
        /// </summary>
        public string[]? CommandLineArgs { get; set; }

        /// <summary>
        ///     Environment name. Generally used "Development", "Staging" or "Production".
        /// </summary>
        public string? EnvironmentName { get; set; }

        /// <summary>
        ///     Prefix for the environment variables.
        /// </summary>
        public string EnvironmentVariablesPrefix { get; set; } = string.Empty;

        /// <summary>
        ///     Used to set assembly which is used to get the user secret id for the application.
        ///     Use this or <see cref="UserSecretsId" /> (higher priority)
        /// </summary>
        public Assembly? UserSecretsAssembly { get; set; }

        /// <summary>
        ///     Used to set user secret id for the application. Use this (higher priority) or <see
        ///     cref="UserSecretsAssembly" />
        /// </summary>
        public string? UserSecretsId { get; set; }
    }
}