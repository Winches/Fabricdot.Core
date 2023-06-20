using Fabricdot.Infrastructure.Data;

namespace Fabricdot.Infrastructure.Tests.Data;

public class ConnectionStringNameAttributeTests : TestBase
{
    [ConnectionStringName(Name)]
    private class Foo
    {
    }

    private class Bar
    {
    }

    private const string Name = nameof(Foo);

    [Fact]
    public void Constructor_GivenInvalidInput_Throw()
    {
        var sut = typeof(ConnectionStringNameAttribute).GetConstructors();

        Create<GuardClauseAssertion>().Verify(sut);
    }

    [Fact]
    public void GetConnStringName_WithAttribute_ReturnCorrectly()
    {
        ConnectionStringNameAttribute.GetConnStringName<Foo>()
                                     .Should()
                                     .Be(Name);
    }

    [Fact]
    public void GetConnStringName_WithoutAttribute_ReturnTypeName()
    {
        var type = typeof(List<>).GetGenericArguments().First();
        var expected = type.FullName ?? type.Name;
        ConnectionStringNameAttribute.GetConnStringName(type)
                                     .Should()
                                     .Be(expected);
    }
}
