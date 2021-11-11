using System.Reflection;
using Fabricdot.Infrastructure.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Mall.Infrastructure.Data
{

    internal static class Schema
    {
        public const string Order = "Orders";
        public const string OrderLine = "OrderLines";
    }
}