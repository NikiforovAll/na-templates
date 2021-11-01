// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Application.IntegrationTests.Projects.Commands;

using NikiforovAll.ES.Template.Application.Projects.Commands.CreateProject;
using NikiforovAll.ES.Template.Application.SharedKernel.Exceptions;
using NikiforovAll.ES.Template.Domain.ProjectAggregate;

[Trait("Category", "Integration")]
public class CreateProjectCommandTests
{
    [Theory]
    [InlineAutoData("")]
    [InlineAutoData("a")]
    [InlineAutoData("aa")]
    public async Task NameIsTooShort_ExceptionThrownAsync(string invalidName, CreateProjectCommand command)
    {
        command.Name = invalidName;
        await FluentActions.Invoking(() =>
            SendAsync(command)).Should().ThrowAsync<ValidationException>();
    }


    [Theory, AutoData]
    public async Task ValidCommand_ProjectCreated(CreateProjectCommand command)
    {
        var id = await SendAsync(command);

        var entity = await FindAsync<Project>(id) ?? default!;

        entity.Should().NotBeNull();
        entity.Name.Should().Be(command.Name);
        entity.Status.Should().Be(ProjectStatus.Complete);
        entity.Items.Should().BeEmpty();
        entity.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(5));
    }
}
