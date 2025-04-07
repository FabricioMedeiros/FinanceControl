using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinanceControl.API.Filters.Swagger
{
    public class PaymentMethodSchemaFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod == "POST"
                && context.ApiDescription.RelativePath?.Contains("paymentmethod", StringComparison.OrdinalIgnoreCase) == true)
            {
                operation.RequestBody.Content["application/json"].Example = new OpenApiObject
                {
                    ["name"] = new OpenApiString("Cartão de Crédito")
                };
            }
        }
    }
}