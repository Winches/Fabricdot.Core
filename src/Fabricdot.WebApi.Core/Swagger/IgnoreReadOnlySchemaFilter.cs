using System.Linq;
using Fabricdot.Common.Core.Enumerable;
using Fabricdot.Infrastructure.Core.Commands;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fabricdot.WebApi.Core.Swagger
{
    /// <summary>
    ///     fix read-only property of ICommand
    /// </summary>
    public class IgnoreReadOnlySchemaFilter : ISchemaFilter
    {
        /// <inheritdoc />
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var commandType = typeof(ICommand<>);
            var isCommand = context.Type.IsClass && context.Type.GetInterfaces()
                .Any(v => v.IsGenericType && v.GetGenericTypeDefinition() == commandType);

            if (!isCommand)
                return;
            schema.ReadOnly = false;
            schema.Properties?.Where(v => v.Key != nameof(CommandBase.Id).ToLower())
                .ForEach(v => v.Value.ReadOnly = false);
        }
    }
}