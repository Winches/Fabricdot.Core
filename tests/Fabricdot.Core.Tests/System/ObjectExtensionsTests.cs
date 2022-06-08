using System;
using System.Globalization;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fabricdot.Core.Tests.System.Reflection;

public class ObjectExtensionsTests
{
    [Fact]
    public void IsNull_GivenInput_ReturnCorrectly()
    {
        (null as object)!.IsNull().Should().BeTrue();
        new object().IsNull().Should().BeFalse();
    }

    [Fact]
    public void Cast_GivenInput_CastObject()
    {
        object obj = 1;
        obj.Cast<int>().Should().Be((int)obj);
    }

    [Fact]
    public void As_GivenInput_CastObjectSafely()
    {
        object obj = 1;

        ObjectExtensions.As<int>(obj).Should().Be((int)obj);
        ObjectExtensions.As<string>(obj).Should().BeNull();
    }

    [Fact]
    public void To_GivenNull_ThrowException()
    {
        FluentActions.Invoking(() => (null as object).To<string>())
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void To_GivenGuidType_ConvertToGuid()
    {
        const string obj = "444AED21-D3F2-4341-B919-4FB6DDA87610";
        obj.To<Guid>().Should().Be(Guid.Parse(obj));
    }

    [Fact]
    public void To_GivenEnumName_ConvertToEnum()
    {
        nameof(ServiceLifetime.Singleton).To<ServiceLifetime>()
                                         .Should()
                                         .Be(ServiceLifetime.Singleton);
    }

    [Fact]
    public void To_GivenInvalidEnumName_ThrowException()
    {
        FluentActions.Invoking(() => "Singleton1".To<ServiceLifetime>())
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