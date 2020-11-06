using System;

namespace Fabricdot.Domain.Core.Auditing
{
    public interface IHasDeletionTime : ISoftDelete
    {
        DateTime? DeletionTime { get; }
    }
}