using System.ComponentModel;
using System.Reflection;

namespace Fabricdot.Core.Tests.System.Reflection;

public class AttributeExtensionsTests : TestBase
{
    [Description]
    private class Foo
    {
        [Description]
        public void Print([Description] string text)
        {
            Console.WriteLine(text);
        }
    }

    [Fact]
    public void IsDefined_GivenType_ReturnCorrectly()
    {
        typeof(Foo).IsDefined<DescriptionAttribute>()
                   .Should()
                   .BeTrue();
    }

    [Fact]
    public void IsDefined_GivenMemberInfo_ReturnCorrectly()
    {
        typeof(Foo).GetMethod(nameof(Foo.Print))!
                   .IsDefined<DescriptionAttribute>()
                   .Should()
                   .BeTrue();
    }

    [Fact]
    public void IsDefined_GivenParameterInfo_ReturnCorrectly()
    {
        typeof(Foo).GetMethod(nameof(Foo.Print))!
                   .GetParameters()[0]
                   .IsDefined<DescriptionAttribute>()
                   .Should()
                   .BeTrue();
    }
}
