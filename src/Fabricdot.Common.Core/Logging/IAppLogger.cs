namespace Fabricdot.Common.Core.Logging
{
    // ReSharper disable once UnusedTypeParameter
    public interface IAppLogger<out T>
    {
        void LogInformation(string message, params object[] args);

        void LogWarning(string message, params object[] args);

        void LogError(string message, params object[] args);
    }
}