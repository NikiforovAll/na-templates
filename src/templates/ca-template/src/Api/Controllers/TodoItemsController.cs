// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using NikiforovAll.CA.Template.Application.Projects.Models;
using NikiforovAll.CA.Template.Application.SharedKernel.Models;
using NikiforovAll.CA.Template.Application.ToDoItems.Queries.SearchToDoItem;
using NSwag.Annotations;

/// <summary>
/// TodoItems
/// </summary>
[OpenApiTag("Items", Description = "Manage todo items.")]
[Route("api/todos")]
public class TodoItemsController : ApiControllerBase
{
    /// <summary>
    /// Searches items.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="cancellationToken"></param>
    [HttpGet]
    [Route("search", Name = nameof(SearchToDoItemsAsync))]
    [ProducesResponseType(typeof(PaginatedList<TodoItemViewModel>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedList<TodoItemViewModel>>> SearchToDoItemsAsync(
        [FromQuery] SearchTodoItemQuery query, CancellationToken cancellationToken) =>
            await this.Mediator.Send(query, cancellationToken);
}
