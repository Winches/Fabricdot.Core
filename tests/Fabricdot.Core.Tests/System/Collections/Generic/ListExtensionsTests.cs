using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.System.Collections.Generic
{
    public class ListExtensionsTests
    {
        [Fact]
        public void MoveItem_GivenSameIndex_DoNothing()
        {
            var list = Enumerable.Range(0, 10).ToList();
            const int value = 0;
            var index = list.IndexOf(value);
            list.MoveItem(v => v == value, index);

            list.IndexOf(value).Should().Be(index);
        }

        [Fact]
        public void MoveItem_GivenNewIndex_ChangeOrder()
        {
            var list = Enumerable.Range(0, 10).ToList();
            const int value = 0;
            var index = list.IndexOf(value) + 1;
            list.MoveItem(v => v == value, index);

            list.IndexOf(value).Should().Be(index);
        }
    }
}