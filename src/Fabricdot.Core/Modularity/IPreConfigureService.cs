namespace Fabricdot.Core.Modularity
{
    public interface IPreConfigureService
    {
        void PreConfigureServices(ConfigureServiceContext context);
    }
}