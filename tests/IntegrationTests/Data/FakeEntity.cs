using Fabricdot.Domain.Core.Auditing;
using Fabricdot.Domain.Core.Entities;

namespace IntegrationTests.Data
{
    public class FakeEntity : EntityBase<string>, IAggregateRoot, ISoftDelete
    {
        /// <summary>
        /// </summary>
        public string Name { get; private set; }

        /// <inheritdoc />
        public bool IsDeleted { get; private set; }

        private FakeEntity()
        {
        }

        public FakeEntity(string id, string name)
        {
            Id = id;
            Name = name;
        }

        public void ChangeName(string name)
        {
            Name = name;
        }
    }
}