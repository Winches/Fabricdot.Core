using Fabricdot.Domain.Entities;

namespace Fabricdot.Infrastructure.Tests.Domain.Events
{
    internal class Employee : AggregateRoot<int>
    {
        public string FirstName { get; private set; }

        public string LastName { get; private set; }

        public string Number { get; private set; }

        public Employee(
            string firstName,
            string lastName,
            string number)
        {
            FirstName = firstName;
            LastName = lastName;
            Number = number;
        }
    }
}