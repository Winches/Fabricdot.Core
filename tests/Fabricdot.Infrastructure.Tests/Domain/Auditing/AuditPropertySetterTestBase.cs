using Fabricdot.Infrastructure.Domain.Auditing;
using Fabricdot.Infrastructure.Security;

namespace Fabricdot.Infrastructure.Tests.Domain.Auditing;

public abstract class AuditPropertySetterTestBase : TestFor<AuditPropertySetter>
{
    protected ICurrentUser CurrentUser { get; }

    protected AuditPropertySetterTestBase()
    {
        var currnetUserMock = InjectMock<ICurrentUser>();
        currnetUserMock.SetupGet(v => v.Id).Returns(Create<string>());
        currnetUserMock.SetupGet(v => v.UserName).Returns(Create<string>());

        CurrentUser = currnetUserMock.Object;
    }
}