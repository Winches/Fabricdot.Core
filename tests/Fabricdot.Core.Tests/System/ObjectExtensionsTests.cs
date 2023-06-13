using System.Globalization;
using Microsoft.Extensions.DependencyInjection;

namespace Fabricdot.Core.Tests.System.Reflection;

public class ObjectExtensionsTests : TestBase
{
    [InlineAutoData]
    [InlineAutoData(null)]
    [Theory]
    public void IsNull_GivenInput_ReturnCorrectly(object? obj)
    {
        var expected = obj == null;
        obj.IsNull().Should().Be(expected);
    }

    [Fact]
    public void Cast_GivenInput_CastObject()
    {
        object obj = Create<int>();

        obj.Cast<int>().Should().Be((int)obj);
    }

    [Fact]
    public void As_GivenInput_CastObjectSafely()
    {
        object obj = Create<int>();

        ObjectExtensions.As<int>(obj).Should().Be((int)obj);
        ObjectExtensions.As<string>(obj).Should().BeNull();
    }

    [Fact]
    public void To_GivenNull_ThrowException()
    {
        Invoking(() => ObjectExtensions.To<string>(null!))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void To_GivenGuidType_ConvertToGuid()
    {
        var obj = Guid.NewGuid().ToString();

        obj.To<Guid>().Should().Be(Guid.Parse(obj));
    }

    [Fact]
    public void To_GivenEnumName_ConvertToEnum()
    {
        var @enum = Create<ServiceLifetime>();

        @enum.ToString()
             .To<ServiceLifetime>()
             .Should()
             .Be(@enum);
    }

    [Fact]
    public void To_GivenInvalidEnumName_ThrowException()
    {
        Invoking(() => Create<string>().To<ServiceLifetime>())
                     .Should()
                     .Throw<ArgumentException>();
    }

    [InlineData(1)]
    [InlineData(true)]
    [InlineData(ServiceLifetime.Singleton)]
    [Theory]
    public void To_GivenInput_ChangeType(IConvertible obj)
    {
        var expected = Convert.ChangeType(obj, typeof(int), CultureInfo.InvariantCulture).Cast<int>();

        obj.To<int>()
           .Should()
           .Be(expected);
    }
}
