using System;

namespace Fabricdot.Domain.Core.Auditing
{
    public interface IHasModificationTime
    {
        DateTime LastModificationTime { get; }
    }
}