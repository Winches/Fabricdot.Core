using System.Data.Common;
using Microsoft.Data.Sqlite;

namespace Fabricdot.Testing;

public static class InMemoryDatabaseHelper
{
    private const string ConnectionString = "Filename=:memory:";

    public static DbConnection CreateConnection()
    {
        var connection = new SqliteConnection(ConnectionString);
        connection.Open();
        return connection;
    }
}
