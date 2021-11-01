// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api;

using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;

internal static partial class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Swagger services and configures the Swagger services.
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="configuration"></param>
    /// <returns>The services with Swagger services added.</returns>
    public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
    {
        var apiVersionProvider = services.BuildServiceProvider()
            .GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var apiVersionDescription in apiVersionProvider.ApiVersionDescriptions)
        {
            services.AddOpenApiDocument((options, serviceProvider) =>
            {
                options.Version = apiVersionDescription.ApiVersion.ToString();
                options.PostProcess = document =>
                {
                    document.Info.Title = "Clean Architecture API";
                    document.Info.Description = "";
                };

                // TODO: add strongly typed options
                var identityProviderUrl = configuration.GetValue<string>("IdentityProvider:ExternalUrl");
                var tokenUrl = $"{identityProviderUrl}{configuration.GetValue<string>("IdentityProvider:TokenEndpoint")}";
                var authorizationEndpoint = $"{identityProviderUrl}{configuration.GetValue<string>("IdentityProvider:AuthorizationEndpoint")}";

                if (identityProviderUrl is not null)
                {
                    identityProviderUrl = identityProviderUrl.EndsWith("/", StringComparison.InvariantCultureIgnoreCase)
                    ? identityProviderUrl
                    : $"{identityProviderUrl}/";
                    options.AddSecurity("oauth2", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.OAuth2,
                        Description = "OAuth2 Client Authorization",
                        Flow = OpenApiOAuth2Flow.Implicit,
                        TokenUrl = tokenUrl,
                        Flows = new OpenApiOAuthFlows()
                        {
                            Implicit = new OpenApiOAuthFlow()
                            {
                                Scopes = new Dictionary<string, string>(),
                                AuthorizationUrl = authorizationEndpoint,
                            },
                        }
                    });
                    // Manually provide generated JWT
                    options.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                    {
                        Type = OpenApiSecuritySchemeType.ApiKey,
                        Name = "Authorization",
                        In = OpenApiSecurityApiKeyLocation.Header,
                        Description = "Type into the textbox: Bearer {your JWT token}."
                    });
                }

                options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("oauth2"));
                options.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
        }
        return services;
    }
}
