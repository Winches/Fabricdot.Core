using System;

namespace Fabricdot.Domain.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; }
    }
}