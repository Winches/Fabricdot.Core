using Fabricdot.Infrastructure.Uow;
using Fabricdot.Infrastructure.Uow.Abstractions;
using Microsoft.AspNetCore.Mvc;

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

        unitOfWork!.Options.IsTransactional.Should().BeFalse();

        return "Success";
    }

    [HttpGet("[action]")]
    [UnitOfWork(true)]
    public string GetWithTransactionalUow()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        AssertBegunUow(unitOfWork);

        unitOfWork!.Options.IsTransactional.Should().BeTrue();

        return "Success";
    }

    [HttpGet("[action]")]
    [UnitOfWork(IsDisabled = true)]
    public string GetWithoutUow()
    {
        _unitOfWorkManager.Available.Should().BeNull();

        return "Success";
    }

    [HttpPost("[action]")]
    public void CreateWithUow()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        AssertBegunUow(unitOfWork);

        unitOfWork!.Options.IsTransactional.Should().BeTrue();
    }

    [HttpPost("[action]")]
    public void ThrowException()
    {
        var unitOfWork = _unitOfWorkManager.Available;
        AssertBegunUow(unitOfWork);
        throw new InvalidOperationException("Something happened.");
    }

    private static void AssertBegunUow(IUnitOfWork? unitOfWork)
    {
        unitOfWork.Should().NotBeNull();
        unitOfWork!.IsActive.Should().BeTrue();
    }
}
