using System;
using Fabricdot.Domain.Core.Entities;
using Xunit;

namespace Fabricdot.Domain.Core.Tests
{
    public class AggregateRootBaseTest
    {
        public class Book : AggregateRootBase<Guid>
        {
        }

        [Fact]
        public void New_GenerateConcurrencyStamp()
        {
            var book = new Book();
            var actual = book.ConcurrencyStamp;
            Assert.NotEmpty(actual);
        }
    }
}