namespace Fabricdot.Core.Modularity;

public interface IPostConfigureService
{
    void PostConfigureServices(ConfigureServiceContext context);
}
