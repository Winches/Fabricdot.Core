using System;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Fabricdot.WebApi.Tests;

public abstract class AspNetCoreTestsBase<TStartup> : IDisposable where TStartup : class
{
    private readonly IHost _host;
    protected IServiceProvider ServiceProvider { get; }
    protected TestServer Server { get; }
    protected HttpClient HttpClient { get; }

    protected AspNetCoreTestsBase()
    {
        _host = CreateHostBuilder().Build();
        _host.Start();

        Server = _host.GetTestServer();
        HttpClient = _host.GetTestClient();
        ServiceProvider = Server.Services;
    }

    public void Dispose() => _host.Dispose();

    protected virtual IHostBuilder CreateHostBuilder()
    {
        return Host.CreateDefaultBuilder()
                   .ConfigureWebHostDefaults(webBuilder =>
                   {
                       webBuilder.UseStartup<TStartup>();
                       webBuilder.UseTestServer();
                   })
                   .ConfigureServices(ConfigureServices);
    }

    protected virtual void ConfigureServices(
        HostBuilderContext context,
        IServiceCollection services)
    {
    }
}