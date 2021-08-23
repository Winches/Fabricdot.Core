using System.Reflection;
using System.Threading.Tasks;
using Fabricdot.Core.Aspects;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Domain.Core.Services;
using Fabricdot.Infrastructure.Core.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.Infrastructure.Core.Uow
{
    [Interceptor(Order = ORDER, Target = typeof(IRepository<>))]
    [UnitOfWork]
    public class UnitOfWorkInterceptor : IInterceptor, ITransientDependency
    {
        public const int ORDER = 99;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public UnitOfWorkInterceptor(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        /// <inheritdoc />
        public async Task InvokeAsync(IInvocationContext invocationContext)
        {
            var interceptorAttribute = invocationContext.Method.GetCustomAttribute<UnitOfWorkAttribute>(true)
                                       ?? invocationContext.Method.DeclaringType
                                                           ?.GetCustomAttribute<UnitOfWorkAttribute>(true);
            using var scope = _serviceScopeFactory.CreateScope();
            var serviceProvider = scope.ServiceProvider;
            //use global isolation level
            var options = serviceProvider.GetRequiredService<IOptions<UnitOfWorkOptions>>().Value;
            interceptorAttribute?.Configure(options);
            //Determine UOW is transactional or not.
            if (interceptorAttribute?.IsTransactional == null)
            {
                var uowTransactionBehaviourProvider = serviceProvider.GetRequiredService<IUnitOfWorkTransactionBehaviourProvider>();
                options.IsTransactional = uowTransactionBehaviourProvider.GetBehaviour(invocationContext.Method.Name);
            }
            var unitOfWorkManager = serviceProvider.GetRequiredService<IUnitOfWorkManager>();

            if (unitOfWorkManager.TryBeginReserved(UnitOfWorkManager.RESERVATION_NAME, options))
            {
                await invocationContext.ProceedAsync();
                return;
            }

            using var unitOfWork = unitOfWorkManager.Begin(options);
            await invocationContext.ProceedAsync();
            await unitOfWork.CommitChangesAsync();
        }
    }
}