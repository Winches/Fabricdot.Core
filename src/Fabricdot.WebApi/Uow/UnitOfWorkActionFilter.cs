using System.Reflection;
using System.Threading.Tasks;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Fabricdot.WebApi.Endpoint;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Fabricdot.WebApi.Uow
{
    [ServiceContract(typeof(UnitOfWorkActionFilter))]
    [Dependency(ServiceLifetime.Scoped)]
    public class UnitOfWorkActionFilter : IAsyncActionFilter
    {
        private readonly UnitOfWorkOptions _options;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUnitOfWorkTransactionBehaviourProvider _unitOfWorkTransactionBehaviourProvider;

        public UnitOfWorkActionFilter(
            IOptions<UnitOfWorkOptions> options,
            IUnitOfWorkManager unitOfWorkManager,
            IUnitOfWorkTransactionBehaviourProvider unitOfWorkTransactionBehaviourProvider)
        {
            _options = options.Value;
            _unitOfWorkManager = unitOfWorkManager;
            _unitOfWorkTransactionBehaviourProvider = unitOfWorkTransactionBehaviourProvider;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var actionDescriptor = context.ActionDescriptor;
            if (actionDescriptor is not ControllerActionDescriptor)
            {
                await next();
                return;
            }

            var method = actionDescriptor.AsControllerActionDescriptor().MethodInfo;
            var unitOfWorkAttribute = UnitOfWorkUtil.GetUnitOfWorkAttribute(method);

            if (unitOfWorkAttribute?.IsDisabled ?? false)
            {
                await next();
                return;
            }

            bool IsSucceed(ActionExecutedContext res) => res.Exception == null || res.ExceptionHandled;
            var options = CreateOptions(method, unitOfWorkAttribute);
            if (_unitOfWorkManager.TryBeginReserved(UnitOfWorkManager.RESERVATION_NAME, options))
            {
                var uow = _unitOfWorkManager.Available;
                var res = await next();
                if (!IsSucceed(res))
                    uow.Dispose();//Rollback changes.
            }
            else
            {
                using var uow = _unitOfWorkManager.Begin(options);
                var res = await next();
                if (IsSucceed(res) && uow.IsActive)
                    await uow.CommitChangesAsync(context.HttpContext.RequestAborted);
            }
        }

        private UnitOfWorkOptions CreateOptions(MethodInfo method, UnitOfWorkAttribute unitOfWorkAttribute)
        {
            //use global isolation level
            var options = _options.Clone();
            unitOfWorkAttribute?.Configure(options);
            //Determine UOW is transactional or not.
            if (unitOfWorkAttribute?.IsTransactional == null)
            {
                options.IsTransactional = _unitOfWorkTransactionBehaviourProvider.GetBehaviour(method.Name);
            }

            return options;
        }
    }
}