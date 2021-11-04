// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Application.IntegrationTests.Projects.Commands;

using NikiforovAll.CA.Template.Application.Projects.Commands.CreateProject;
using NikiforovAll.CA.Template.Application.SharedKernel.Exceptions;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;
using NikiforovAll.CA.Template.Domain.ProjectAggregate.Events;
using NikiforovAll.CA.Template.Domain.ValueObjects;

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
        command.ColourCode = Colour.Red;
        var project = await SendAsync(command);

        var entity = await FindAsync<Project>(project.Id) ?? default!;

        entity.Should().NotBeNull();
        entity.Name.Should().Be(command.Name);
        entity.Status.Should().Be(ProjectStatus.Complete);
        entity.Items.Should().BeEmpty();
        entity.Created.Should().BeCloseTo(
            DateTime.UtcNow, TimeSpan.FromSeconds(10));
    }
}
