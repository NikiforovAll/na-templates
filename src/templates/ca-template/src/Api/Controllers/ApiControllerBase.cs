// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api.Controllers;

using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Base Controller
/// </summary>
[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    private ISender? mediator;
    private IPublishEndpoint? publishEndpoint;

    /// <summary>
    /// Mediator isntance.
    /// </summary>
    protected ISender Mediator => this.mediator ??= this.HttpContext.RequestServices.GetRequiredService<ISender>();

    /// <summary>
    /// Endpoint Provider instance.
    /// </summary>
    protected IPublishEndpoint PublishEndpoint =>
        this.publishEndpoint ??= this.HttpContext.RequestServices.GetRequiredService<IPublishEndpoint>();
}
