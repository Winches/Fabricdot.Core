using System;
using Ardalis.GuardClauses;

namespace Fabricdot.Core.Modularity
{
    internal static class GuardClausesExtensions
    {
        internal static Type InvalidModuleType(
            this IGuardClause guard,
            Type input,
            string parameterName)
        {
            if (guard is null)
                throw new ArgumentNullException(nameof(guard));

            _ = guard.Null(input, parameterName);
            return typeof(IModule).IsAssignableFrom(input)
                ? input
                : throw new ArgumentException($"{parameterName} is not a module type.");
        }
    }
}