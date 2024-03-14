using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;

public class DefaultValuesOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var parameter in operation.Parameters)
        {
            var description = context.ApiDescription.ParameterDescriptions.First(p => p.Name == parameter.Name);
            var routeInfo = description.RouteInfo;

            if (parameter.Name == "take" && parameter.In == ParameterLocation.Query)
            {
                parameter.Schema.Default = new OpenApiInteger(10);
            }
            else if (parameter.Name == "skip" && parameter.In == ParameterLocation.Header)
            {
                parameter.Schema.Default = new OpenApiInteger(0);
            }
        }
    }
}
