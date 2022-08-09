using Fabricdot.Domain.ValueObjects;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Fabricdot.WebApi.Swagger;

public class EnumerationSchemaFilter : ISchemaFilter
{
    public void Apply(OpenApiSchema schema, SchemaFilterContext context)
    {
        if (context.Type.IsAssignableTo(typeof(Enumeration)))
        {
            schema.Type = "integer";
            schema.Format = "int32";
        }
    }
}