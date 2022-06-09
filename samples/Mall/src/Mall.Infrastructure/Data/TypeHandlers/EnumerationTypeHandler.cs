using System.Data;
using Dapper;
using Fabricdot.Domain.ValueObjects;

namespace Mall.Infrastructure.Data.TypeHandlers;

internal class EnumerationTypeHandler<T> : SqlMapper.TypeHandler<T> where T : Enumeration
{
    /// <inheritdoc />
    public override void SetValue(IDbDataParameter parameter, T value)
    {
        parameter.Value = value.Value;
        parameter.DbType = DbType.Int32;
    }

    /// <inheritdoc />
    public override T Parse(object value)
    {
        return Enumeration.FromValue<T>((int)value);
    }
}