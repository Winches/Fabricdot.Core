using Fabricdot.Domain.Core.Entities;
using Xunit;

namespace UnitTests.Domain.Entities
{
    public class EntityTest
    {
        public class Person : EntityBase<string>
        {
            public string Name { get; private set; }

            public Person(string id, string name)
            {
                Id = id;
                Name = name;
            }

            public void ChangeName(string name)
            {
                Name = name;
            }
        }

        [Fact]
        public void TestEquals()
        {
            var mike = new Person("1", "mike");
            var jack = new Person("2", "jack");

            Assert.Equal(mike, new Person("1", "mike1"));
            Assert.NotEqual(mike, jack);
            jack.ChangeName("mike");
            Assert.NotEqual(mike, jack);
        }
    }
}