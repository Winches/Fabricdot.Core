using System;
using System.Data;

namespace Fabricdot.Infrastructure.Data
{
    public abstract class SqlConnectionFactory : ISqlConnectionFactory, IDisposable
    {
        private readonly string _connectionString;
        private IDbConnection _connection;

        public SqlConnectionFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <inheritdoc />
        ~SqlConnectionFactory()
        {
            Dispose(false);
        }

        public IDbConnection GetOpenConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = CreateConnection(_connectionString);
                _connection.Open();
            }

            return _connection;
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected abstract IDbConnection CreateConnection(string connectionString);

        protected virtual void Dispose(bool disposing)
        {
            if (disposing && _connection?.State == ConnectionState.Open)
                _connection.Dispose();
        }
    }
}