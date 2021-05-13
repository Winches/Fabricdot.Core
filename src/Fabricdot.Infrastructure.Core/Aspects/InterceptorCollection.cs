using System;
using System.Collections.Generic;
using Fabricdot.Core.Aspects;
using JetBrains.Annotations;

namespace Fabricdot.Infrastructure.Core.Aspects
{
    public class InterceptorCollection : HashSet<InterceptorDescriptor>
    {
        public void Add([NotNull] Type interceptorType)
        {
            if (interceptorType == null)
                throw new ArgumentNullException(nameof(interceptorType));

            var descriptor = InterceptorDescriptor.Create(interceptorType);
            if (!Add(descriptor))
                throw new ArgumentException($"Interceptor of {interceptorType.Name} already registered.");
        }

        public void Add<TInterceptor>() where TInterceptor : IInterceptor => Add(typeof(TInterceptor));
    }
}