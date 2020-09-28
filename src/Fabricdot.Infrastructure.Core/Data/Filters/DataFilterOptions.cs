using System;
using System.Collections.Generic;

namespace Fabricdot.Infrastructure.Core.Data.Filters
{
    public class DataFilterOptions
    {
        public Dictionary<Type, DataFilterState> DefaultStates { get; } = new Dictionary<Type, DataFilterState>();
    }
}