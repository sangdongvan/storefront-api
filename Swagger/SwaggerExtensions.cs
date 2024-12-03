using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Storefront.Swagger;

public static class SwaggerExtensions
{
    public static IApplicationBuilder UseSimpleSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        return app;
    }

    public static IServiceCollection AddSimpleSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            // Fix non-nullable type
            options.SupportNonNullableReferenceTypes();

            // Supports Bearer Token
            options.AddSecurityDefinition("BearerToken",
                new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    Description = "Authorization header using the Bearer scheme."
                });

            options.OperationFilter<AuthorizeCheckOperationFilter>();
        });

        return services;
    }

    private sealed class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var metadata = context.ApiDescription.ActionDescriptor.EndpointMetadata;

            if (!metadata.OfType<IAuthorizeData>().Any())
            {
                return;
            }

            operation.Responses.TryAdd("401", new OpenApiResponse { Description = "Unauthorized" });
            operation.Responses.TryAdd("403", new OpenApiResponse { Description = "Forbidden" });

            var bearerToken = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "BearerToken" }
            };

            operation.Security = new List<OpenApiSecurityRequirement>
            {
                new() { [bearerToken] = Array.Empty<string>() }
            };
        }
    }
}