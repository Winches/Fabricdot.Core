using System.Linq;
using System.Threading.Tasks;
using Fabricdot.Core.Aspects;

namespace Fabricdot.Infrastructure.Core.Tests.Aspects.Interceptors
{
    internal class IntegerParameterInterceptor : IParameterInterceptor
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