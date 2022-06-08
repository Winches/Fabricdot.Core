using System.Data;

namespace Fabricdot.Infrastructure.Data;

public interface ISqlConnectionFactory
{
    IDbConnection GetOpenConnection();
}