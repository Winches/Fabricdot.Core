using Fabricdot.Domain.Auditing;
using Fabricdot.Domain.Entities;

namespace Fabricdot.Domain.Tests.Entities;

public class AuditPropertiesTests : TestBase
{
    [AutoMockData]
    [Theory]
    public void AggregateRoot_Should_HaveWriteableProperties(FullAuditAggregateRoot<Guid> entity)
    {
        var properties = new[]
        {
            nameof(IHasCreationTime.CreationTime),
            nameof(IHasCreatorId.CreatorId),
            nameof(IHasModificationTime.LastModificationTime),
            nameof(IHasModifierId.LastModifierId),
            nameof(IHasDeletionTime.DeletionTime),
            nameof(IHasDeleterId.DeleterId),
        };
        var type = entity.GetType();

        properties.ForEach(p => type.GetProperty(p).Should().BeWritable());
    }

    [AutoMockData]
    [Theory]
    public void Entity_Should_HaveWriteableProperties(FullAuditEntity<Guid> entity)
    {
        var properties = new[]
        {
            nameof(IHasCreationTime.CreationTime),
            nameof(IHasCreatorId.CreatorId),
            nameof(IHasModificationTime.LastModificationTime),
            nameof(IHasModifierId.LastModifierId),
            nameof(IHasDeletionTime.DeletionTime),
            nameof(IHasDeleterId.DeleterId),
        };
        var type = entity.GetType();

        properties.ForEach(p => type.GetProperty(p).Should().BeWritable());
    }
}