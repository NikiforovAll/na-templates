// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Text.Json;
using System.Text.Json.Nodes;

internal static class EndpointRouteBuilderExtensions
{
    /// <summary>
    /// Adds a Health Check endpoint to the <see cref="IEndpointRouteBuilder"/> with the specified template.
    /// </summary>
    /// <param name="endpoints">The <see cref="IEndpointRouteBuilder"/> to add endpoint to.</param>
    /// <param name="pattern">The URL pattern of the endpoint.</param>
    /// <param name="servicesPattern"></param>
    /// <returns>A route for the endpoint.</returns>
    public static IEndpointConventionBuilder MapCustomHealthCheck(
        this IEndpointRouteBuilder endpoints, string pattern, string servicesPattern)
    {
        if (endpoints == null)
        {
            throw new ArgumentNullException(nameof(endpoints));
        }

        endpoints.MapHealthChecks(pattern, new HealthCheckOptions()
        {
            ResponseWriter = WriteResponse,
            Predicate = (check) => false,
            AllowCachingResponses = false,
        });
        return endpoints.MapHealthChecks(servicesPattern, new HealthCheckOptions()
        {
            ResponseWriter = WriteResponse,
            Predicate = (check) => true,
            AllowCachingResponses = true,
        });

        static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JsonObject()
            {
                ["status"] = result.Status.ToString(),

            };
            if (result.Entries.Any())
            {
                json["results"] = new JsonArray(result.Entries.Select(e => new JsonObject
                {
                    ["name"] = e.Key,
                    ["status"] = e.Value.Status.ToString(),
                }).ToArray());
            }

            return context.Response.WriteAsync(
                json.ToJsonString(new JsonSerializerOptions { WriteIndented = true }));
        }
    }
}
