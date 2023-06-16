namespace Fabricdot.Core.Tests.System.Collections.Generic;

public class ListExtensionsTests : TestFor<List<int>>
{
    [Fact]
    public void MoveItem_GivenSameIndex_DoNothing()
    {
        var value = Sut.OrderBy(_ => Guid.NewGuid()).First();
        var index = Sut.IndexOf(value);
        Sut.MoveItem(v => v == value, index);

        Sut.IndexOf(value).Should().Be(index);
    }

    [Fact]
    public void MoveItem_GivenNewIndex_ChangeOrder()
    {
        var value = Sut.Skip(1).OrderBy(_ => Guid.NewGuid()).First();
        var index = Sut.IndexOf(value) - 1;
        Sut.MoveItem(v => v == value, index);

        Sut.IndexOf(value).Should().Be(index);
    }
}
