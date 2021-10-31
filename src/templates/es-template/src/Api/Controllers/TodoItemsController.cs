// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Api.Controllers;

using Microsoft.AspNetCore.Mvc;
using Nikiforovall.ES.Template.Application.Projects.Models;
using Nikiforovall.ES.Template.Application.SharedKernel.Models;
using Nikiforovall.ES.Template.Application.ToDoItems.Queries.SearchToDoItem;
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
