// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.CA.Template.Domain.UnitTests;

using System;
using AutoFixture.Xunit2;
using FluentAssertions;
using NikiforovAll.CA.Template.Domain.ProjectAggregate;
using NikiforovAll.CA.Template.Domain.ProjectAggregate.Events;
using NikiforovAll.CA.Template.Domain.ValueObjects;

public class ProjectTests
{
    [Theory, AutoData]
    public void ProjectConstructed_Sucessfully(string name)
    {
        var colour = Colour.Yellow;
        var project = new Project(name, colour);

        project.Colour.Should().Be(colour);
        project.Name.Should().Be(name);
        project.Items.Should().BeEmpty();
    }

    [Fact]
    public void ProjectConstructed_InvalidName_ExceptionThrown() =>
        FluentActions.Invoking(() => new Project(string.Empty, Colour.White))
            .Should().Throw<ArgumentException>()
                .Which.Message.Contains(nameof(Project.Name));

    [Theory, AutoData]
    public void AddItem_AddedSucessfully(Project project, ToDoItem item)
    {
        project.AddItem(item);

        project.Items.Should().ContainEquivalentOf(item);
        project.Items.Should().HaveCount(1);
        project.DomainEvents.Should().ContainSingle()
            .Which.GetType().Name.Should().Be(nameof(NewItemAddedEvent));
    }

    [Theory, AutoData]
    public void AddItem_InvalidItem_ExceptionThrown(Project project) =>
        FluentActions.Invoking(() => project.AddItem(default!));

    [Theory, AutoData]
    public void UpdateName_UpdatedSucessfully(Project project, string newProjectName)
    {
        project.UpdateName(newProjectName);

        project.Name.Should().Be(newProjectName);
    }

    [Theory, AutoData]
    public void UpdateName_InvalidName_ExceptionThrown(Project project) =>
        FluentActions.Invoking(() => project.UpdateName(String.Empty))
            .Should().Throw<ArgumentException>();
}
