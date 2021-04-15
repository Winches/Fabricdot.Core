using System;
using Fabricdot.Domain.Core.Entities;
using Xunit;

namespace Fabricdot.Domain.Core.Tests
{
    public class EntityBaseTest
    {
        internal class Person : EntityBase<Guid>
        {
            public string Name { get; private set; }

            public Person(Guid id, string name)
            {
                Id = id;
                Name = name;
            }
        }

        [Fact]
        public void Equal_GivenInstanceWithSameId_ReturnTrue()
        {
            var id = Guid.NewGuid();
            var person1 = new Person(id, "mike");
            var person2 = new Person(id, "mike");
            Assert.True(person1.Equals(person2));
        }

        [Fact]
        public void Equal_GivenInstanceWithDifferentId_ReturnFalse()
        {
            var person1 = new Person(Guid.NewGuid(), "mike");
            var person2 = new Person(Guid.NewGuid(), "mike");
            Assert.False(person1.Equals(person2));
        }

        [Fact]
        public void Equal_GivenNull_ReturnFalse()
        {
            var person = new Person(Guid.NewGuid(), "mike");
            Assert.False(person.Equals(null));
        }
    }
}