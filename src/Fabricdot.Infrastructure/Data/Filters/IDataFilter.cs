using System;

namespace Fabricdot.Infrastructure.Data.Filters;

public interface IDataFilter
{
    IDisposable Enable<TFilter>() where TFilter : class;

    IDisposable Disable<TFilter>() where TFilter : class;

    bool IsEnabled<TFilter>() where TFilter : class;
}

public interface IDataFilter<TFilter> where TFilter : class
{
    bool IsEnabled { get; }

    IDisposable Enable();

    IDisposable Disable();
}