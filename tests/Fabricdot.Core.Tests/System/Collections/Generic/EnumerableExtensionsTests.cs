using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.System.Collections.Generic
{
    public class EnumerableExtensionsTests
    {
        public static IEnumerable<object[]> GetEnumrables()
        {
            yield return new object[] { null };
            yield return new object[] { Array.Empty<object>() };
            yield return new object[] { new List<object>() };
            yield return new object[] { new object[] { 1 } };
        }

        [MemberData(nameof(GetEnumrables))]
        [Theory]
        public void IsNullOrEmpty_GivenInput_ReturnCorrectly(IEnumerable<object> enumerable)
        {
            var isNullOrEmpty = enumerable?.Any() != true;
            enumerable.IsNullOrEmpty().Should().Be(isNullOrEmpty);
        }

        [Fact]
        public void ForEach_GivenAction_IteratingElements()
        {
            var collection = Enumerable.Range(1, 10).ToList();
            var count = 0;
            collection.ForEach(_ => count++);

            count.Should().Be(collection.Count);
            collection.ForEach((v, i) => v.Should().Be(collection[i]));
        }

        [Fact]
        public async Task ForEachAsync_GivenAction_IteratingElementsAsync()
        {
            var collection = Enumerable.Range(1, 10).ToList();
            var count = 0;
            await collection.ForEachAsync((v, i) =>
            {
                Task.Delay(10);
                count++;
                v.Should().Be(collection[i]);
                return Task.CompletedTask;
            });

            count.Should().Be(collection.Count());
        }

        [Fact]
        public void JoinAsString_GivenInput_JoinString()
        {
            const string separator1 = "__";
            const char separator2 = '_';
            var source = new[] { " a", " -", " c" };
            var expected1 = string.Join(separator1, source);
            var expected2 = string.Join(separator2, source);

            source.JoinAsString(separator1)
                  .Should()
                  .Be(expected1);
            source.JoinAsString(separator2)
                  .Should()
                  .Be(expected2);
        }
    }
}