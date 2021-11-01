// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Application.IntegrationTests.Projects.Queries;

using Nikiforovall.CA.Template.Application.Projects.Queries.GetProject;
using Nikiforovall.CA.Template.Application.SharedKernel.Exceptions;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;
using Nikiforovall.CA.Template.Tests.Common;

[Trait("Category", "Integration")]
public class GetProjectByIdQueryTests : IntegrationTestBase
{
    [Theory, AutoData]
    public void ProjectDoesNotExist_ExceptionThrow(GetProjectByIdQuery query) =>
        FluentActions.Invoking(() =>
            SendAsync(query)).Should().ThrowAsync<NotFoundException>();

    [Theory, AutoProjectData]
    public async Task ExistingProjectWithToDoItems_Retrieved(Project project, IList<ToDoItem> items)
    {
        foreach (var i in items)
        {
            project.AddItem(i);
        }
        await InsertAsync(project);

        var query = new GetProjectByIdQuery() { Id = project.Id };

        var projectViewModel = await SendAsync(query);

        projectViewModel.Should().NotBeNull();
        projectViewModel.Name.Should().Be(project.Name);
        projectViewModel.DisplayName.Should().Contain(project.Colour);
        items.Should().BeEquivalentTo(projectViewModel.Items);
    }
}
