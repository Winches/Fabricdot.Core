using System;

namespace Fabricdot.Domain.Core.Auditing
{
    public interface IHasCreationTime
    {
        DateTime CreationTime { get; }
    }
}