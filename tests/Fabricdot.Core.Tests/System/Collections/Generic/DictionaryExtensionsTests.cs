using System.Collections.Generic;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.System.Collections.Generic;

public class DictionaryExtensionsTests
{
    [Fact]
    public void GetOrDefault_WhenValueIsNotNull_ReturnValue()
    {
        const string key = "Name1";
        const int value = 1;
        var dictionary = new Dictionary<string, int>()
        {
            { key,value }
        };
        dictionary.GetOrDefault(key).Should().Be(value);
    }

    [Fact]
    public void GetOrDefault_WhenValueIsNull_ReturnDefaultValue()
    {
        const string key = "Name1";
        const int value = default;
        var dictionary = new Dictionary<string, int>();
        dictionary.GetOrDefault(key).Should().Be(value);
    }

    [Fact]
    public void GetOrAdd_WhenKeyExists_ReturnValue()
    {
        const string key = "Name1";
        const int value = 1;
        var dictionary = new Dictionary<string, int>()
        {
            { key,value }
        };

        dictionary.GetOrAdd(key, _ => value).Should().Be(value);
    }

    [Fact]
    public void GetOrAdd_WhenKeyNotExists_AddValue()
    {
        const string key = "Name1";
        const int value = 1;
        var dictionary = new Dictionary<string, int>();

        dictionary.GetOrAdd(key, _ => value).Should().Be(value);
    }
}