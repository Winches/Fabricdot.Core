using System.Data;
using Fabricdot.Core.DependencyInjection;
using Fabricdot.Infrastructure.Data;
using Microsoft.Data.SqlClient;

namespace Mall.Infrastructure.Data;

public class DefaultSqlConnectionFactory : SqlConnectionFactory, IScopedDependency
{
    public DefaultSqlConnectionFactory(string connectionString) : base(connectionString)
    {
    }

    protected override IDbConnection CreateConnection(string connectionString)
    {
        // Create db connection.
        return new SqlConnection(connectionString);
    }
}