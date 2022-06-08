using System;
using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Fabricdot.WebApi.Tests.Uow;

[Route("api/fake-uow")]
[ApiController]
public class FakeUnitOfWorkController : ControllerBase
{
    private readonly IUnitOfWorkManager _unitOfWorkManager;

    public FakeUnitOfWorkController(IUnitOfWorkManager unitOfWorkManager)
    {
        _unitOfWorkManager = unitOfWorkManager;
    }

    [HttpGet("[action]")]
    public string GetWithUow()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        AssertBegunUow(unitOfWork);
        var isTransactional = unitOfWork.Options.IsTransactional;
        Assert.False(isTransactional);

        return "Success";
    }

    [HttpGet("[action]")]
    [UnitOfWork(true)]
    public string GetWithTransactionalUow()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        AssertBegunUow(unitOfWork);
        var isTransactional = unitOfWork.Options.IsTransactional;
        Assert.True(isTransactional);

        return "Success";
    }

    [HttpGet("[action]")]
    [UnitOfWork(IsDisabled = true)]
    public string GetWithoutUow()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        Assert.Null(unitOfWork);

        return "Success";
    }

    [HttpPost("[action]")]
    public void CreateWithUow()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        AssertBegunUow(unitOfWork);
        var isTransactional = unitOfWork.Options.IsTransactional;
        Assert.True(isTransactional);
    }

    [HttpPost("[action]")]
    public void ThrowException()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        AssertBegunUow(unitOfWork);
        throw new InvalidOperationException("Something happened.");
    }

    private static void AssertBegunUow(IUnitOfWork unitOfWork)
    {
        Assert.NotNull(unitOfWork);
        Assert.True(unitOfWork.IsActive);
    }
}