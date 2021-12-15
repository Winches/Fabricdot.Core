using System.Collections.Generic;
using Fabricdot.Domain.ValueObjects;
using Xunit;

namespace Fabricdot.Domain.Tests
{
    public class ValueObjectTest
    {
        internal class PersonName : ValueObject
        {
            public string FirstName { get; }

            public string LastName { get; }

            public PersonName(string firstName, string lastName)
            {
                FirstName = firstName;
                LastName = lastName;
            }

            /// <inheritdoc />
            protected override IEnumerable<object> GetAtomicValues()
            {
                yield return FirstName;
                yield return LastName;
            }
        }

        [Fact]
        public void Equal_GivenSameValueInstance_ReturnTrue()
        {
            var name1 = new PersonName("Allen", "Yeager");
            var name2 = new PersonName("Allen", "Yeager");
            Assert.True(name1.Equals(name2));
        }

        [Fact]
        public void Equal_GivenDifferentValueInstance_ReturnFalse()
        {
            var name1 = new PersonName("Allen", "Yeager");
            var name2 = new PersonName("Allen", "Jason");
            Assert.False(name1.Equals(name2));
        }

        [Fact]
        public void Equal_GivenNull_ReturnFalse()
        {
            var name = new PersonName("Allen", "Yeager");
            Assert.False(name.Equals(null));
        }
    }
}