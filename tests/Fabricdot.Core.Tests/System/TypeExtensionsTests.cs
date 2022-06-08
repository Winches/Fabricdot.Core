using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.System;

public class TypeExtensionsTests
{
    private class IntCollection : List<int>
    {
    }

    [SpecialName]
    private class SpecialType
    {
    }

    [InlineData(typeof(IntCollection), typeof(List<>), true)]
    [InlineData(typeof(IntCollection), typeof(ICollection<>), true)]
    [InlineData(typeof(IntCollection), typeof(IDictionary<,>), false)]
    [InlineData(typeof(List<int>), typeof(ICollection<>), true)]
    [InlineData(typeof(List<int>), typeof(IDictionary<,>), false)]
    [Theory]
    public void IsAssignableToGenericType_GivenInput_ReturnCorrectly(
        Type type,
        Type genericType,
        bool expected)
    {
        type.IsAssignableToGenericType(genericType)
            .Should()
            .Be(expected);
    }

    [Fact]
    public void IsAssignableToGenericType_GivenNull_ThrowException()
    {
        FluentActions.Invoking(() => (null as Type).IsAssignableToGenericType(typeof(ICollection<>)))
                     .Should()
                     .Throw<ArgumentNullException>();

        FluentActions.Invoking(() => typeof(IList<int>).IsAssignableToGenericType(null))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsInNamespace_GivenNullType_ThrowException()
    {
        FluentActions.Invoking(() => (null as Type).IsInNamespace(null))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [Theory]
    public void IsInNamespace_GivenInvalidNamespace_ThrowException(string @namespace)
    {
        FluentActions.Invoking(() => typeof(int).IsInNamespace(@namespace))
                     .Should()
                     .Throw<ArgumentException>();
    }

    [InlineData(typeof(int), nameof(System))]
    [InlineData(typeof(int), "System.Collections")]
    [Theory]
    public void IsInNamespace_GivenInput_ReturnCorrectly(Type type, string @namespace)
    {
        var expected = type.Namespace.StartsWith(@namespace);
        type.IsInNamespace(@namespace).Should().Be(expected);
    }

    [Fact]
    public void IsInNamespaces_GivenNull_ThrowException()
    {
        FluentActions.Invoking(() => (null as Type).IsInNamespaces(null))
                     .Should()
                     .Throw<ArgumentNullException>();

        FluentActions.Invoking(() => typeof(int).IsInNamespaces(null))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void IsNonAbstractClass_GivenNull_ThrowException()
    {
        FluentActions.Invoking(() => (null as Type).IsNonAbstractClass(false))
            .Should()
            .Throw<ArgumentNullException>();
    }

    [InlineData(typeof(IntCollection), true, false)]
    [InlineData(typeof(IntCollection), false, true)]
    [InlineData(typeof(PublicNonNestedType), true, true)]
    [InlineData(typeof(SpecialType), false, false)]
    [Theory]
    public void IsNonAbstractClass_GivenInput_ReturnCorrectly(
        Type type,
        bool publicOnly,
        bool expected)
    {
        type.IsNonAbstractClass(publicOnly).Should().Be(expected);
    }

    [InlineData(typeof(IntCollection), nameof(IntCollection))]
    [InlineData(typeof(List<int>), "List<Int32>")]
    [InlineData(typeof(List<KeyValuePair<string, int>>), "List<KeyValuePair<String,Int32>>")]
    [InlineData(typeof(List<KeyValuePair<string, int[]>>), "List<KeyValuePair<String,Int32[]>>")]
    [InlineData(typeof(List<List<List<List<List<int>>>>>), "List<List<List<List<List`1>>>>")]
    [InlineData(typeof(List<>), "List<>")]
    [Theory]
    public void PrettyPrint_GivenType_ReturnPrettyTypeName(Type type, string expected)
    {
        type.PrettyPrint().Should().Be(expected);
    }

    [Fact]
    public void GetCacheKey_GivenType_ReturnCorrectly()
    {
        var type = typeof(IntCollection);
        var expected = $"{type.PrettyPrint()}[hash: {type.GetHashCode()}]";
        type.GetCacheKey().Should().Be(expected);
    }
}

public class PublicNonNestedType
{
}