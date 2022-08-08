namespace Fabricdot.Core.Tests.System.Collections.Generic;

public class CollectionExtensionsTests : TestBase
{
    [Fact]
    public void RemoveAll_GivenNull_ThrowException()
    {
        Invoking(() => ((ICollection<object>)null).RemoveAll(_ => true))
                     .Should()
                     .Throw<ArgumentNullException>();

        Invoking(() => new List<object>().RemoveAll(null))
                     .Should()
                     .Throw<ArgumentNullException>();
    }

    [Fact]
    public void RemoveAll_GivenPredicate_RemoveElements()
    {
        var collection = Enumerable.Range(1, 10).ToList();
        Func<int, bool> Predicate = v => v <= 5;

        collection.RemoveAll(Predicate)
                  .Should()
                  .OnlyContain(v => Predicate(v));
        collection.Should()
                  .OnlyContain(v => !Predicate(v));
    }
}