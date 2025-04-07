using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinanceControl.API.Filters.Swagger
{
    public class CategorySchemaFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod == "POST"
                && context.ApiDescription.RelativePath?.Contains("category", StringComparison.OrdinalIgnoreCase) == true)
            {
                operation.RequestBody.Content["application/json"].Example = new OpenApiObject
                {
                    ["name"] = new OpenApiString("Alimentação"),
                    ["description"] = new OpenApiString("Despesas com comida"),
                    ["type"] = new OpenApiInteger(0) // Exemplo para o enum CategoryType
                };
            }
        }
    }
}