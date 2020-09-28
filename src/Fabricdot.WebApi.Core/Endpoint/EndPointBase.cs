using System;
using AutoMapper;
using Fabricdot.Common.Core.Security;
using Fabricdot.Infrastructure.Core.Data;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabricdot.WebApi.Core.Endpoint
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EndPointBase : ControllerBase
    {
        protected readonly object ServiceProviderLock = new object();
        private ILoggerFactory _loggerFactory;

        private IMapper _mapper;
        private IUnitOfWork _unitOfWork;

        private ICurrentUser _currentUser;
        private IMediator _mediator;
        public IServiceProvider ServiceProvider => HttpContext.RequestServices;
        protected ILoggerFactory LoggerFactory => LazyGetRequiredService(ref _loggerFactory);

        protected ILogger Logger => LazyLogger.Value;

        protected IMapper Mapper => LazyGetRequiredService(ref _mapper);
        protected IUnitOfWork UnitOfWork => LazyGetRequiredService(ref _unitOfWork);

        protected ICurrentUser CurrentUser => LazyGetRequiredService(ref _currentUser);
        protected IMediator Mediator => LazyGetRequiredService(ref _mediator);

        private Lazy<ILogger> LazyLogger =>
            new Lazy<ILogger>(() => LoggerFactory?.CreateLogger(GetType().FullName), true);

        protected TService LazyGetRequiredService<TService>(ref TService reference)
        {
            return LazyGetRequiredService(typeof(TService), ref reference);
        }

        protected TRef LazyGetRequiredService<TRef>(Type serviceType, ref TRef reference)
        {
            if (reference == null)
                lock (ServiceProviderLock)
                {
                    reference ??= (TRef) ServiceProvider.GetRequiredService(serviceType);
                }

            return reference;
        }
    }
}