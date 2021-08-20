using System;
using System.Collections.Generic;

namespace Fabricdot.Infrastructure.Core.Data
{
    [Serializable]
    public class ConnectionStrings : Dictionary<string, string>
    {
        public const string DEFAULT_NAME = "Default";

        public string Default
        {
            get => this.GetValueOrDefault(DEFAULT_NAME);
            set => this[DEFAULT_NAME] = value;
        }
    }
}