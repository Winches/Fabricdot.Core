using System;
using AutoMapper;
using Fabricdot.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Fabricdot.WebApi.Endpoint
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EndPointBase : ControllerBase
    {
        protected readonly object ServiceProviderLock = new object();

        private IMapper _mapper = null!;
        private ICurrentUser _currentUser = null!;
        private ISender _sender = null!;
        private ILogger<object> _logger = null!;

        public IServiceProvider ServiceProvider => HttpContext.RequestServices;

        protected ILogger<object> Logger => LazyGetRequiredService(typeof(ILogger<>).MakeGenericType(GetType()), ref _logger);
        protected IMapper Mapper => LazyGetRequiredService(ref _mapper);
        protected ICurrentUser CurrentUser => LazyGetRequiredService(ref _currentUser);
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