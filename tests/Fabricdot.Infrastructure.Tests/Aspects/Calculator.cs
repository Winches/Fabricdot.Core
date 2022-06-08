using Fabricdot.Core.DependencyInjection;

namespace Fabricdot.Infrastructure.Tests.Aspects;

public class Calculator : ICalculator, ITransientDependency
{
    /// <inheritdoc />
    public virtual int Plus(int left, int right) => left + right;

    /// <inheritdoc />
    public virtual int Minus(int left, int right) => left - right;

    /// <inheritdoc />
    public virtual int Multiply(int left, int right) => left * right;

    /// <inheritdoc />
    public virtual int Divide(int left, int right) => left / right;
}