using System;
using Dapper;
using Fabricdot.Core.Reflection;
using Fabricdot.Domain.Core.ValueObjects;
using Mall.Domain.Entities.OrderAggregate;

namespace Mall.Infrastructure.Data.TypeHandlers
{
    public static class SqlMapperTypeHandlerConfiguration
    {
        public static void AddTypeHandlers()
        {
            //register enumeration handlers
            var enumerationType = ReflectionHelper.FindTypes(typeof(Enumeration), typeof(OrderStatus).Assembly);
            enumerationType
                .ForEach(v =>
                {
                    var handlerType = typeof(EnumerationTypeHandler<>).MakeGenericType(v);
                    var typeHandler = (SqlMapper.ITypeHandler)Activator.CreateInstance(handlerType);
                    SqlMapper.AddTypeHandler(v, typeHandler);
                });
        }
    }
}