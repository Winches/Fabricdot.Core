using Fabricdot.Core.Reflection;

namespace Fabricdot.Core.Tests;

public class ReflectionHelperTests : TestBase
{
    internal interface IAnimal
    {
    }

    internal interface IAnimal<in T>
    {
    }

    internal interface IWolf : IAnimal
    {
    }

    internal interface IRabbit : IAnimal
    {
    }

    internal abstract class WolfBase : IWolf, IAnimal<IRabbit>
    {
    }

    internal class GreyWolf : WolfBase
    {
    }

    internal class Rabbit : IRabbit
    {
    }

    [Fact]
    public void FindTypes_GivenDirectlyInheritedInterface_ReturnDerivedTypes()
    {
        var expected = typeof(GreyWolf);
        var types = ReflectionHelper.FindTypes(typeof(IWolf), typeof(ReflectionHelperTests).Assembly);

        types.SingleOrDefault().Should().Be(expected);
    }

    [Fact]
    public void FindTypes_GivenIndirectlyInheritedInterface_ReturnDerivedTypes()
    {
        var types = ReflectionHelper.FindTypes(typeof(IAnimal), typeof(ReflectionHelperTests).Assembly);

        types.Should().Contain(typeof(GreyWolf));
        types.Should().Contain(typeof(Rabbit));
    }

    [Fact]
    public void FindTypes_GivenGenericInterface_ReturnDerivedTypes()
    {
        var types = ReflectionHelper.FindTypes(typeof(IAnimal<>), typeof(ReflectionHelperTests).Assembly);
        var expected = typeof(GreyWolf);

        types.SingleOrDefault().Should().Be(expected);
    }

    [Fact]
    public void FindTypes_GivenDirectlyInheritedClass_ReturnDerivedTypes()
    {
        var expected = typeof(GreyWolf);
        var types = ReflectionHelper.FindTypes(typeof(WolfBase), typeof(ReflectionHelperTests).Assembly);

        types.SingleOrDefault().Should().Be(expected);
    }
}
