using System.Data;

namespace Fabricdot.Infrastructure.Core.Data
{
    public interface ISqlConnectionFactory
    {
        IDbConnection GetOpenConnection();
    }
}