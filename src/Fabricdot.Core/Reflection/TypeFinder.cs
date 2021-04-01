using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Fabricdot.Core.Reflection
{
    public class TypeFinder : ITypeFinder
    {
        private const string SKIP_ASSEMBLIES = "^System|^Mscorlib|^msvcr120|^Netstandard|^Microsoft|^Autofac|^AutoMapper|^EntityFramework|^Newtonsoft|^Castle|^NLog|^Pomelo|^AspectCore|^Xunit|^Nito|^Npgsql|^Exceptionless|^MySqlConnector|^Anonymously Hosted|^libuv|^api-ms|^clrcompression|^clretwrc|^clrjit|^coreclr|^dbgshim|^e_sqlite3|^hostfxr|^hostpolicy|^MessagePack|^mscordaccore|^mscordbi|^mscorrc|sni|sos|SOS.NETCore|^sos_amd64|^SQLitePCLRaw|^StackExchange|^Swashbuckle|WindowsBase|ucrtbase|^DotNetCore.CAP|^MongoDB|^Confluent.Kafka|^librdkafka|^EasyCaching|^RabbitMQ|^Consul|^Dapper|^EnyimMemcachedCore|^Pipelines|^DnsClient|^IdentityModel|^zlib|^JetBrains|^MediatR|^Skia|^Polly|^Ardalis|^Senparc";

        private static readonly string ApplicationName = Assembly.GetEntryAssembly()?.GetName().Name;

        /// <summary>
        /// Get assemblies without ignored assemblies
        /// </summary>
        public virtual List<Assembly> GetAssemblies()
        {
            var applicationBasePath = AppDomain.CurrentDomain.BaseDirectory;
            LoadAssemblies(applicationBasePath);
            return GetAssembliesFromCurrentAppDomain();
        }

        private void LoadAssemblies(string path)
        {
            foreach (string file in Directory.GetFiles(path, "*.dll"))
            {
                if (Match(Path.GetFileName(file)) == false)
                    continue;

                try
                {
                    var assemblyName = AssemblyName.GetAssemblyName(file);
                    AppDomain.CurrentDomain.Load(assemblyName);
                }
                catch (BadImageFormatException)
                {
                }
            }
        }

        protected virtual bool Match(string assemblyName)
        {
            if (assemblyName.StartsWith($"{ApplicationName}.Views"))
                return false;
            if (assemblyName.StartsWith($"{ApplicationName}.PrecompiledViews"))
                return false;
            return !Regex.IsMatch(assemblyName, SKIP_ASSEMBLIES, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        protected bool Match(Assembly assembly)
        {
            return !Regex.IsMatch(assembly.FullName ?? string.Empty, SKIP_ASSEMBLIES, RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        private List<Assembly> GetAssembliesFromCurrentAppDomain()
        {
            var result = new List<Assembly>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                if (Match(assembly))
                    result.Add(assembly);
            }
            return result.Distinct().ToList();
        }

        /// <inheritdoc />
        public List<Type> Find<T>(params Assembly[] assemblies)
        {
            return Find(typeof(T), assemblies);
        }

        /// <inheritdoc />
        public List<Type> Find(Type findType, params Assembly[] assemblies)
        {
            assemblies ??= GetAssemblies().ToArray();
            return ReflectionHelper.FindTypes(findType, assemblies);
        }
    }
}
