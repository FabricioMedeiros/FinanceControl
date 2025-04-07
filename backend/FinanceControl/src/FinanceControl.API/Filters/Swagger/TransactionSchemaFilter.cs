using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FinanceControl.API.Filters.Swagger
{
    public class TransactionSchemaFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.RelativePath?.Contains("transaction", StringComparison.OrdinalIgnoreCase) == true)
            {
                var commonProperties = new OpenApiObject
                {
                    ["amount"] = new OpenApiDouble(100.50),
                    ["date"] = new OpenApiString("2025-04-03T23:04:08.537Z"),
                    ["description"] = new OpenApiString("Pagamento de conserto do carro."),
                    ["category"] = new OpenApiObject
                    {
                        ["id"] = new OpenApiString("3fa85f64-5717-4562-b3fc-2c963f66afa6")
                    },
                    ["paymentMethod"] = new OpenApiObject
                    {
                        ["id"] = new OpenApiString("3fa85f64-5717-4562-b3fc-2c963f66afa6")
                    }
                };

                if (context.ApiDescription.HttpMethod == "POST")
                {
                    operation.RequestBody.Content["application/json"].Example = commonProperties;
                }
                else if (context.ApiDescription.HttpMethod == "PUT")
                {
                    var exampleForUpdate = new OpenApiObject
                    {
                        ["id"] = new OpenApiString("3fa85f64-5717-4562-b3fc-2c963f66afa6") 
                    };

                    foreach (var property in commonProperties)
                    {
                        exampleForUpdate[property.Key] = property.Value; 
                    }

                    operation.RequestBody.Content["application/json"].Example = exampleForUpdate;
                }
            }
        }
    }
}

//using Microsoft.OpenApi.Any;
//using Microsoft.OpenApi.Models;
//using Swashbuckle.AspNetCore.SwaggerGen;

//namespace FinanceControl.API.Filters.Swagger
//{
//    public class TransactionSchemaFilter : IOperationFilter
//    {
//        public void Apply(OpenApiOperation operation, OperationFilterContext context)
//        {
//            if (context.ApiDescription.HttpMethod == "POST"
//                && context.ApiDescription.RelativePath?.Contains("transaction", StringComparison.OrdinalIgnoreCase) == true)
//            {
//                operation.RequestBody.Content["application/json"].Example = new OpenApiObject
//                {
//                    ["amount"] = new OpenApiDouble(100.50),
//                    ["date"] = new OpenApiString("2025-04-03T23:04:08.537Z"),
//                    ["description"] = new OpenApiString("Pagamento de serviço"),
//                    ["category"] = new OpenApiObject
//                    {
//                        ["id"] = new OpenApiString("3fa85f64-5717-4562-b3fc-2c963f66afa6")
//                    },
//                    ["paymentMethod"] = new OpenApiObject
//                    {
//                        ["id"] = new OpenApiString("3fa85f64-5717-4562-b3fc-2c963f66afa6")
//                    }
//                };
//            }
//        }
//    }
//}