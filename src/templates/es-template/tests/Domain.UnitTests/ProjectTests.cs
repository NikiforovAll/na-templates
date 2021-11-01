// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Domain.UnitTests;

using Nikiforovall.ES.Template.Domain.ProjectAggregate;
using Nikiforovall.ES.Template.Domain.ValueObjects;
using Nikiforovall.ES.Template.Tests.Common;
using static Nikiforovall.ES.Template.Domain.ProjectAggregate.Events.Events.V1;

public class ProjectTests
{
    [Theory, AutoData]
    public void ProjectConstructed_Sucessfully(Guid id, string name)
    {
        var colour = Colour.Yellow;
        var project = new Project(id, name, colour);

        project.ShouldBeCreatedWith(id, name, colour);
    }

    [Theory, AutoData]
    public void ProjectConstructed_InvalidName_ExceptionThrown(Guid id) =>
        FluentActions.Invoking(() => new Project(id, string.Empty, Colour.White))
            .Should().Throw<ArgumentException>()
                .Which.Message.Contains(nameof(Project.Name));

    [Theory, AutoProjectData]
    public void AddItem_AddedSucessfully(Project project, ToDoItem item)
    {
        item.ProjectId = project.Id;
        project.AddItem(item);

        project.Items.Should().HaveCount(1);

        item.ProjectNumber = 1;
        project.Items.Should().ContainEquivalentOf(item);

        var expectedEvent = NewItemAdded.Create(
            item.Id, 1, item.Title, item.Description, project.Id);

        project.PublishedEvent<NewItemAdded>().Should()
            .BeEquivalentTo(expectedEvent);
    }

    [Theory, AutoData]
    public void AddItem_InvalidItem_ExceptionThrown(Project project) =>
        FluentActions.Invoking(() => project.AddItem(default!));

    [Theory, AutoData]
    public void UpdateName_UpdatedSucessfully(Project project, string newProjectName)
    {
        project.UpdateName(newProjectName);

        project.Name.Should().Be(newProjectName);

        var expectedEvent = NameUpdated.Create(newProjectName);

        project.PublishedEvent<NameUpdated>().Should()
            .BeEquivalentTo(expectedEvent);
    }

    [Theory, AutoData]
    public void UpdateName_InvalidName_ExceptionThrown(Project project) =>
        FluentActions.Invoking(() => project.UpdateName(string.Empty))
            .Should().Throw<ArgumentException>();
}
