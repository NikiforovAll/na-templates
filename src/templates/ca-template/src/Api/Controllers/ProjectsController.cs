// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using NikiforovAll.CA.Template.Application.Projects.Commands.CreateProject;
using NikiforovAll.CA.Template.Application.Projects.Models;
using NikiforovAll.CA.Template.Application.Projects.Queries.GetProject;
using NikiforovAll.CA.Template.Application.Projects.Queries.GetProjects;
using NikiforovAll.CA.Template.Application.SharedKernel.Models;
using NikiforovAll.CA.Template.Messaging.Contracts;
using NSwag.Annotations;

/// <summary>
/// Projects
/// </summary>
[OpenApiTag("Projects", Description = "Manage projects.")]
[Route("api/projects")]
public class ProjectsController : ApiControllerBase
{
    /// <summary>
    /// Get paged projects.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    [Route("", Name = nameof(GetProjectsAsync))]
    [ProducesResponseType(typeof(PaginatedList<ProjectSummaryViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<ProjectSummaryViewModel>>> GetProjectsAsync(
        [FromQuery] GetProjectsWithPaginationQuery query, CancellationToken cancellationToken) =>
            await this.Mediator.Send(query, cancellationToken);

    /// <summary>
    /// Get a project by Id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Entity is returned.</response>
    /// <response code="404">Entity is not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpGet]
    [Route("{id}", Name = nameof(GetProjectAsync))]
    [ProducesResponseType(typeof(ProjectViewModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProjectViewModel>> GetProjectAsync(
        Guid id, CancellationToken cancellationToken)
    {
        var request = new GetProjectByIdQuery { Id = id };
        return await this.Mediator.Send(request, cancellationToken);
    }

    /// <summary>
    /// Creates a project.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="cancellationToken"></param>
    /// <response code="200">Entity is returned.</response>
    /// <response code="404">Entity is not found.</response>
    /// <response code="500">Internal server error.</response>
    [HttpPost]
    [Route("", Name = nameof(CreateProjectAsync))]
    [ProducesResponseType(StatusCodes.Status202Accepted)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> CreateProjectAsync(
        CreateProjectCommand command, CancellationToken cancellationToken)
    {
        await this.PublishEndpoint.Publish<ICreateProject>(
            new
            {
                command.Name,
                Colour = command.ColourCode
            }, cancellationToken);

        return this.Accepted();
    }
}
