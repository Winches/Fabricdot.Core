using System.Collections.Generic;
using Fabricdot.Domain.Core.ValueObjects;
using Xunit;

namespace UnitTests.Domain.ValueObjects
{
    public class ValueObjectTest
    {
        public class TestValueObject : ValueObject
        {
            /// <inheritdoc />
            public TestValueObject(string name, int value)
            {
                Name = name;
                Value = value;
            }

            public string Name { get; }

            public int Value { get; }

            /// <inheritdoc />
            protected override IEnumerable<object> GetAtomicValues()
            {
                yield return Name;
                yield return Value;
            }
        }

        [Fact]
        public void TestEquals()
        {
            var vo1 = new TestValueObject("A", 1);
            var vo2 = new TestValueObject("B", 1);
            var vo3 = new TestValueObject("A", 1);

            Assert.Equal(vo1, vo3);
            Assert.NotEqual(vo1, vo2);
        }
    }
}