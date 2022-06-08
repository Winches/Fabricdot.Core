using System;

namespace Fabricdot.Domain.Auditing;

public interface IHasDeletionTime : ISoftDelete
{
    DateTime? DeletionTime { get; }
}