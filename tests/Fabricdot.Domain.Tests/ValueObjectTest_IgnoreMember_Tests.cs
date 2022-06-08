using Fabricdot.Domain.SharedKernel;
using Fabricdot.Domain.ValueObjects;
using Xunit;

namespace Fabricdot.Domain.Tests;

public class ValueObjectTest_IgnoreMember_Tests
{
    internal class PersonName : ValueObject
    {
        public string FirstName { get; }

        public string LastName { get; }

        [IgnoreMember]
        public string MiddleName { get; }

        public PersonName(
            string firstName,
            string lastName,
            string middleName = null)
        {
            FirstName = firstName;
            LastName = lastName;
            MiddleName = middleName;
        }
    }

    [Fact]
    public void Equal_GivenDifferentValue_IgnoreMember()
    {
        var name1 = new PersonName("Allen", "Yeager", "Idiot");
        var name2 = new PersonName("Allen", "Yeager");
        Assert.True(name1.Equals(name2));
        Assert.True(name1 == name2);
        Assert.False(name1 != name2);
    }
}