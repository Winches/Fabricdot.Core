using System.Threading.Tasks;
using Fabricdot.Core.Aspects;
using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Aspects.Interceptors
{
    [LoggingInterceptor]
    internal class LoggingInterceptor : IInterceptor, ITransientDependency
    {
        public static bool IsLogged { get; set; }

        /// <inheritdoc />
        public async Task InvokeAsync(IInvocationContext invocationContext)
        {
            IsLogged = true;
            await invocationContext.ProceedAsync();
        }
    }
}