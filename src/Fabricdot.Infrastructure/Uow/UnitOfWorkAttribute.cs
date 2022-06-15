using System;
using System.Data;
using Fabricdot.Core.Aspects;
using Fabricdot.Infrastructure.Uow.Abstractions;

namespace Fabricdot.Infrastructure.Uow;

[InterceptorBinding]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class UnitOfWorkAttribute : Attribute
{
    public bool? IsTransactional { get; set; }

    public IsolationLevel? IsolationLevel { get; set; }

    public bool IsDisabled { get; set; }

    public UnitOfWorkAttribute()
    {
    }

    public UnitOfWorkAttribute(bool isTransactional)
    {
        IsTransactional = isTransactional;
    }

    public UnitOfWorkAttribute(bool isTransactional, IsolationLevel isolationLevel)
    {
        IsTransactional = isTransactional;
        IsolationLevel = isolationLevel;
    }

    public void Configure(UnitOfWorkOptions options)
    {
        options.IsTransactional = IsTransactional ?? options.IsTransactional;
        options.IsolationLevel = IsolationLevel ?? options.IsolationLevel;
    }
}