using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using AspectCore.DynamicProxy;
using Fabricdot.Core.Aspects;
using JetBrains.Annotations;

namespace Fabricdot.Infrastructure.Aspects.AspectCore
{
    [DisableAspect]
    public class AspectCoreInvocationContextAdapter : IInvocationContext
    {
        [NotNull]
        private readonly AspectContext _context;

        [NotNull]
        private readonly AspectDelegate _next;

        /// <inheritdoc />
        public object TargetObject => _context.Implementation;

        /// <inheritdoc />
        public MethodInfo Method => _context.ImplementationMethod;

        /// <inheritdoc />
        public object[] Parameters => _context.Parameters;

        /// <inheritdoc />
        public object ReturnValue { get; set; }

        /// <inheritdoc />
        public IDictionary<object, object> Properties { get; }

        public AspectCoreInvocationContextAdapter([NotNull] AspectContext context, [NotNull] AspectDelegate next)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _next = next ?? throw new ArgumentNullException(nameof(next));

            ReturnValue = context.ReturnValue;
            Properties = new Dictionary<object, object>();
            foreach (var (key, value) in context.AdditionalData)
                Properties.Add(key, value);
        }

        /// <inheritdoc />
        public virtual async Task ProceedAsync()
        {
            await _next(_context);
            ReturnValue = _context.ReturnValue;
        }
    }
}