using System;

namespace Fabricdot.MultiTenancy.Abstractions;

public interface ITenant
{
    Guid Id { get; }

    string Name { get; }
}