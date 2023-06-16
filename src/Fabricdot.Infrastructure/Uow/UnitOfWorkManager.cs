using Ardalis.GuardClauses;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Infrastructure.Uow;

public class UnitOfWorkManager : IUnitOfWorkManager, ISingletonDependency
{
    public const string RESERVATION_NAME = "DefaultReservedUnitOfWork";
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly IAmbientUnitOfWork _ambientUnitOfWork;

    public IUnitOfWork? Available => _ambientUnitOfWork.Find(v => v.IsActive);

    public UnitOfWorkManager(IServiceProvider serviceProvider)
    {
        _serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        _ambientUnitOfWork = serviceProvider.GetRequiredService<IAmbientUnitOfWork>();
    }

    /// <inheritdoc />
    public IUnitOfWork Begin(
        UnitOfWorkOptions? options = null,
        bool requireNew = false)
    {
        var availableUow = Available;
        if (availableUow != null && !requireNew)
        {
            return new ChildUnitOfWork(availableUow);
        }

        var unitOfWork = CreateUnitOfWork();
        unitOfWork.Initialize(options ?? new UnitOfWorkOptions());
        return unitOfWork;
    }

    public void BeginReserved(
        string name,
        UnitOfWorkOptions? options = null)
    {
        if (!TryBeginReserved(name, options))
            throw new InvalidOperationException($"There is no reserved unit of work for '{name}'");
    }

    public bool TryBeginReserved(
        string name,
        UnitOfWorkOptions? options = null)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        var uow = _ambientUnitOfWork.Find(v => v.IsReservedFor(name));
        uow?.Initialize(options ?? new UnitOfWorkOptions());
        return uow != null;
    }

    public IUnitOfWork Reserve(
        string name,
        bool requireNew = false)
    {
        Guard.Against.NullOrEmpty(name, nameof(name));

        var uow = _ambientUnitOfWork.UnitOfWork;
        if (!requireNew && uow != null && uow.IsReservedFor(name))
        {
            return new ChildUnitOfWork(uow);
        }

        var unitOfWork = CreateUnitOfWork();
        unitOfWork.Reserve(name);
        return unitOfWork;
    }

    private IUnitOfWork CreateUnitOfWork()
    {
        var serviceScope = _serviceScopeFactory.CreateScope();

        try
        {
            var uow = serviceScope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            _ambientUnitOfWork.UnitOfWork = uow;

            uow.Disposed += (_, _) =>
             {
                 _ambientUnitOfWork.DropCurrent();
                 // ReSharper disable once AccessToDisposedClosure
                 serviceScope.Dispose();
             };
            return uow;
        }
        catch
        {
            serviceScope.Dispose();
            throw;
        }
    }
}
