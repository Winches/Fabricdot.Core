using System;
using System.Data;
using Microsoft.Data.SqlClient;

namespace Fabricdot.Infrastructure.Core.Data
{
    public class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IDbConnection GetOpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
                if (_connection != null && _connection.State == ConnectionState.Open)
                    _connection.Dispose();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        ~SqlConnectionFactory()
        {
            Dispose(false);
        }
    }
}