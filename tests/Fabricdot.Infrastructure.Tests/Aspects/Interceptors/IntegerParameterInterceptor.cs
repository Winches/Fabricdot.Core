using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Core.Aspects;
using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Aspects.Interceptors
{
    [ServiceContract(typeof(IParameterInterceptor))]
    internal class IntegerParameterInterceptor : IParameterInterceptor, ITransientDependency
    {
        public static int[] Parameters { get; set; }

        /// <inheritdoc />
        public async Task InvokeAsync(IInvocationContext invocationContext)
        {
            Parameters = invocationContext.Parameters.OfType<int>().ToArray();
            await invocationContext.ProceedAsync();
        }
    }
}