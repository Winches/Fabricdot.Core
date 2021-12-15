using System;
using System.Reflection;
using Ardalis.GuardClauses;
using AspectCore.Configuration;
using Fabricdot.Core.Aspects;
using Fabricdot.Infrastructure.Aspects.AspectCore.Configuration;

namespace Fabricdot.Infrastructure.Aspects.AspectCore
{
    public static class AspectPredicateUtil
    {
        private static readonly AspectPredicate NonPredicate = _ => false;

        public static AspectPredicate IsDefined<TAttribute>(bool inherit) where TAttribute : Attribute =>
            IsDefined(typeof(TAttribute), inherit);

        public static AspectPredicate IsDefined(Type attributeType, bool inherit)
        {
            Guard.Against.Null(attributeType, nameof(attributeType));
            if (!attributeType.IsAssignableTo(typeof(Attribute)))
                throw new ArgumentException("The target type must be an attribute.");

            return method =>
            {
                var declaringType = method.DeclaringType;
                if (declaringType is not null && declaringType.IsDefined(attributeType, inherit))
                    return true;

                return method.IsDefined(attributeType, inherit);
            };
        }

        public static AspectPredicate IsAssignableTo(Type targetType)
        {
            Guard.Against.Null(targetType, nameof(targetType));
            if (!targetType.IsClass && !targetType.IsInterface)
                throw new ArgumentException("The target type must be class or interface.");
            if (targetType.IsSealed)
                throw new ArgumentException("The target type is not allowed to be sealed.");

            return method =>
            {
                var declaringType = method.DeclaringType;
                return declaringType is not null
                       && (declaringType.IsAssignableTo(targetType)
                           || declaringType.IsAssignableToGenericType(targetType));
            };
        }

        public static AspectPredicate CreatePredicate(InterceptorDescriptor interceptorDescriptor)
        {
            var predicate = IsDefined<DisableAspectAttribute>(true)
                .Not();

            if (interceptorDescriptor.TargetType == null && interceptorDescriptor.BindingType == null)
                return predicate;

            var targetPredicate = interceptorDescriptor.TargetType != null
                ? IsAssignableTo(interceptorDescriptor.TargetType)
                : NonPredicate;
            var attributePredicate = interceptorDescriptor.BindingType != null
                ? IsDefined(
                    interceptorDescriptor.BindingType,
                    true)
                : NonPredicate;

            return predicate.And(targetPredicate.Or(attributePredicate));
        }
    }
}