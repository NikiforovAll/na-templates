// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Application.ToDoItems.Queries.SearchToDoItem;

using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Nikiforovall.ES.Template.Application.Interfaces;
using Nikiforovall.ES.Template.Application.Projects.Models;
using Nikiforovall.ES.Template.Application.SharedKernel.Mappings;
using Nikiforovall.ES.Template.Application.SharedKernel.Models;
using Nikiforovall.ES.Template.Application.SharedKernel.Utils;
using Nikiforovall.ES.Template.Domain.ProjectAggregate.Specifications;

public class SearchTodoItemQuery : IRequest<PaginatedList<TodoItemViewModel>>
{
    public string SearchTerm { get; set; } = default!;

    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}

public class SearchTodoItemQueryHandler
    : IRequestHandler<SearchTodoItemQuery, PaginatedList<TodoItemViewModel>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public SearchTodoItemQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<TodoItemViewModel>> Handle(
        SearchTodoItemQuery request, CancellationToken cancellationToken)
    {
        var spec = new ItemsSearchSpecification(request.SearchTerm);

        var query = this.context.ToDoItems
            .ApplySpecification(spec)
            .AsNoTracking()
            .OrderBy(i => i.Id);

        var items = await query
            .ProjectTo<TodoItemViewModel>(this.mapper.ConfigurationProvider)
            .PaginatedListAsync(request.PageNumber, request.PageSize);

        return items;
    }
}
