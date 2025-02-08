namespace Ajloun_Tour
{
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.SwaggerGen;

    public class FileUploadOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            // Look for IFormFile parameters in the action parameters
            var fileParams = context.MethodInfo.GetParameters()
                .Where(p => p.ParameterType == typeof(IFormFile) ||
                            p.ParameterType == typeof(IFormFileCollection))
                .ToList();

            if (fileParams.Any())
            {
                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = {
                    ["multipart/form-data"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = fileParams.ToDictionary(
                                p => p.Name,
                                p => new OpenApiSchema
                                {
                                    Type = "string",
                                    Format = "binary"
                                })
                        }
                    }
                }
                };
            }
        }
    }

}
