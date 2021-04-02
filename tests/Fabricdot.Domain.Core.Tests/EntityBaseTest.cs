using System;
using System.Linq;
using Fabricdot.Domain.Core.Entities;
using Fabricdot.Domain.Core.Events;
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

        [Fact]
        public void AddDomainEvent_GivenEvent_Correctly()
        {
            var person = new Person(Guid.NewGuid(), "mike");
            var expected = new EntityCreatedEvent<Person>(person);
            person.AddDomainEvent(expected);
            var actual = person.DomainEvents.Single();

            Assert.Equal(expected, actual);
        }

        [Fact]
        public void AddDomainEvent_GivenNull_ThrownException()
        {
            var person = new Person(Guid.NewGuid(), "mike");
            void Action() => person.AddDomainEvent(null);
            Assert.Throws<ArgumentNullException>(Action);
        }

        [Fact]
        public void RemoveDomainEvent_GivenEvent_Correctly()
        {
            var person = new Person(Guid.NewGuid(), "mike");
            var expected = new EntityCreatedEvent<Person>(person);
            person.AddDomainEvent(expected);
            person.RemoveDomainEvent(expected);

            Assert.DoesNotContain(expected, person.DomainEvents);
        }

        [Fact]
        public void ClearDomainEvents_EmptyEvents()
        {
            var person = new Person(Guid.NewGuid(), "mike");
            var domainEvent = new EntityCreatedEvent<Person>(person);
            person.AddDomainEvent(domainEvent);
            person.ClearDomainEvents();

            Assert.Empty(person.DomainEvents);
        }
    }
}