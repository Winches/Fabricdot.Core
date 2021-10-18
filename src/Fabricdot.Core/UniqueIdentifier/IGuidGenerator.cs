using System;

namespace Fabricdot.Core.UniqueIdentifier
{
    public interface IGuidGenerator
    {
        Guid Create();
    }
}