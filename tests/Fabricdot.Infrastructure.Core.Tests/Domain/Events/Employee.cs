using Fabricdot.Domain.Core.Entities;

namespace Fabricdot.Infrastructure.Core.Tests.Domain.Events
{
    internal class Employee : AggregateRootBase<int>
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