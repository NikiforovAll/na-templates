// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            Predicate = (check) => !check.Tags.Contains("services"),
            AllowCachingResponses = false,
        });
        return endpoints.MapHealthChecks(servicesPattern, new HealthCheckOptions()
        {
            ResponseWriter = WriteResponse,
            Predicate = (check) => true,
            AllowCachingResponses = false,
        });

        static Task WriteResponse(HttpContext context, HealthReport result)
        {
            context.Response.ContentType = "application/json";

            var json = new JObject(
                new JProperty("status", result.Status.ToString()),
                new JProperty("results", new JObject(result.Entries.Select(pair =>
                    new JProperty(pair.Key, new JObject(
                        new JProperty("status", pair.Value.Status.ToString()),
                        new JProperty("description", pair.Value.Description),
                        new JProperty("data", new JObject(pair.Value.Data.Select(
                            p => new JProperty(p.Key, p.Value))))))))));

            return context.Response.WriteAsync(
                json.ToString(Formatting.Indented));
        }
    }
}
