// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.ToDoItems.Queries.SearchToDoItem;

using AutoMapper;
using MediatR;
using NikiforovAll.ES.Template.Application.Projects.Models;
using NikiforovAll.ES.Template.Application.SharedKernel.Mappings;
using NikiforovAll.ES.Template.Application.SharedKernel.Models;
using NikiforovAll.ES.Template.Application.SharedKernel.Repositories;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;
using NikiforovAll.ES.Template.Domain.ProjectAggregate.Specifications;

public class SearchTodoItemQuery : IRequest<PaginatedList<TodoItemViewModel>>
{
    public string SearchTerm { get; set; } = default!;

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SearchTodoItemQueryHandler
    : IRequestHandler<SearchTodoItemQuery, PaginatedList<TodoItemViewModel>>
{
    private readonly IDocumentStore store;
    private readonly IMapper mapper;

    public SearchTodoItemQueryHandler(
        IDocumentStore store, IMapper mapper)
    {
        this.store = store ?? throw new ArgumentNullException(nameof(store));
        this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<PaginatedList<TodoItemViewModel>> Handle(
        SearchTodoItemQuery request,
        CancellationToken cancellationToken)
    {
        var spec = new ItemsSearchSpecification(request.SearchTerm);

        var query = this.store.Query<ToDoItem>().ApplySpecification(spec);

        var paged = await query
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        var mapped = this.mapper.Map<List<TodoItemViewModel>>(paged.Items);
        return new PaginatedList<TodoItemViewModel>(
            mapped, paged.TotalCount, paged.PageIndex, paged.TotalPages);
    }
}
