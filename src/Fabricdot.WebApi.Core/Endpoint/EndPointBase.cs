using System;
using AutoMapper;
using Fabricdot.Common.Core.Logging;
using Fabricdot.Common.Core.Security;
using Fabricdot.Infrastructure.Core.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.WebApi.Core.Endpoint
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EndPointBase : ControllerBase
    {
        protected readonly object ServiceProviderLock = new object();

        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;
        private ICurrentUser _currentUser;
        private IMediator _mediator;
        private ISender _sender;
        private IAppLogger<object> _logger;

        public IServiceProvider ServiceProvider => HttpContext.RequestServices;

        protected IAppLogger<object> Logger => LazyGetRequiredService(typeof(IAppLogger<>).MakeGenericType(GetType()), ref _logger);
        protected IMapper Mapper => LazyGetRequiredService(ref _mapper);
        protected IUnitOfWork UnitOfWork => LazyGetRequiredService(ref _unitOfWork);
        protected ICurrentUser CurrentUser => LazyGetRequiredService(ref _currentUser);
        [Obsolete("use Sender")]
        protected IMediator Mediator => LazyGetRequiredService(ref _mediator);
        protected ISender Sender => LazyGetRequiredService(ref _sender);

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            return LazyGetRequiredService(typeof(TService), ref reference);
        }

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
                lock (ServiceProviderLock)
                {
                    reference ??= (TRef)ServiceProvider.GetRequiredService(serviceType);
                }

            return reference;
        }
    }
}