using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Xunit;

namespace Fabricdot.Core.Tests.System.Collections.Generic;

public class CollectionExtensionsTests
{
    [Fact]
    public void RemoveAll_GivenNull_ThrowException()
    {
        FluentActions.Invoking(() => ((ICollection<object>)null).RemoveAll(_ => true))
                     .Should()
                     .Throw<ArgumentNullException>();

        FluentActions.Invoking(() => new List<object>().RemoveAll(null))
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