using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace BoldareApp.Infrastructure
{
    public class ODataQueryOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Dodaj tylko dla v2
            var version = context.ApiDescription.GroupName?.ToLower();
            if (version != "v2") return;

            // Ogranicz do kontrolera breweries
            if (!operation.Tags.Any(t => t.Name.ToLower().Contains("breweries"))) return;

            operation.Parameters ??= new List<OpenApiParameter>();

            string[] odataParams = { "$filter", "$orderby", "$top", "$skip" };

            foreach (var param in odataParams)
            {
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = param,
                    In = ParameterLocation.Query,
                    Required = false,
                    Description = $"OData query option: {param}",
                    Schema = new OpenApiSchema { Type = "string" }
                });
            }
        }
    }
}
