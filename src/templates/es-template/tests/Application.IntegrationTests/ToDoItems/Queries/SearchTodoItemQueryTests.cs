// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.IntegrationTests.ToDoItems.Queries;

using NikiforovAll.ES.Template.Application.Projects.Models;
using NikiforovAll.ES.Template.Application.SharedKernel.Exceptions;
using NikiforovAll.ES.Template.Application.ToDoItems.Queries.SearchToDoItem;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;
using NikiforovAll.ES.Template.Tests.Common;

[Trait("Category", "Integration")]
public class SearchTodoItemQueryTests : IntegrationTestBase
{
    [Theory]
    [MemberData(nameof(InvalidSearchCommands))]
    public async Task InvalidQueries_ExceptionThrownAsync(SearchTodoItemQuery query) =>
        await FluentActions.Invoking(() =>
            SendAsync(query)).Should().ThrowAsync<ValidationException>();

    public static IEnumerable<object[]> InvalidSearchCommands()
    {
        var fixture = new Fixture();
        var validPageNumber = fixture.Create<int>();
        var validPageSize = fixture.Create<int>();
        var validSearchTearm = fixture.Create<string>();

        yield return new object[]
        {
            new SearchTodoItemQuery()
            {
                PageNumber = int.MinValue,
                PageSize = validPageSize,
                SearchTerm = validSearchTearm,
            }
        };
        yield return new object[]
        {
            new SearchTodoItemQuery()
            {
                PageNumber = validPageNumber,
                PageSize = int.MinValue,
                SearchTerm = validSearchTearm,
            }
        };
        yield return new object[]
        {
            new SearchTodoItemQuery()
            {
                PageNumber = validPageNumber,
                PageSize = validPageSize,
                SearchTerm = string.Empty,
            }
        };
    }

    [Theory, AutoProjectData]
    public async Task ExistingToDoItemSearchByTitle_Found(
        Project project,
        Project project2,
        IList<ToDoItem> items,
        IList<ToDoItem> items2)
    {
        foreach (var i in items)
        {
            project.AddItem(i);
        }
        foreach (var i in items2)
        {
            project2.AddItem(i);
        }
        var itemToFind = items.First();
        project.MarkComplete(itemToFind.ProjectNumber);
        await InsertAsync(project, project2);

        var query = new SearchTodoItemQuery() { SearchTerm = itemToFind.Title };

        var foundItems = await SendAsync(query);

        foundItems.TotalCount.Should().Be(1);
        foundItems.Items.Should().ContainSingle().Which.Should().BeEquivalentTo(new TodoItemViewModel
        {
            ProjectNumber = 1,
            Description = itemToFind.Description,
            IsDone = itemToFind.IsDone,
            Title = itemToFind.Title,
        });
    }

    [Theory, AutoProjectData]
    public async Task TryToSearchForNonExistingItem_EmptyResultReturned(Project project, IList<ToDoItem> items, string randomTitle)
    {
        foreach (var i in items)
        {
            project.AddItem(i);
        }
        await InsertAsync(project);

        var query = new SearchTodoItemQuery() { SearchTerm = randomTitle };

        var foundItems = await SendAsync(query);

        foundItems.TotalCount.Should().Be(0);
        foundItems.Items.Should().BeEmpty();
    }
}
