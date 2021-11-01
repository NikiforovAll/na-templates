// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Domain.UnitTests;

using Nikiforovall.ES.Template.Domain.ProjectAggregate;

public class ToDoItemTests
{
    [Theory, AutoData]
    public void ToDoItemConstructed_Sucessfully(Project project, string title, string description)
    {
        var sut = new ToDoItem(Guid.NewGuid(), 1, project.Id)
        {
            Description = description,
            Title = title,
        };

        sut.Title.Should().Be(title);
        sut.Description.Should().Be(description);
        sut.IsDone.Should().BeFalse();
    }

    [Theory, AutoData]
    public void MarkComplete_CompletedAdnEventDispatched(ToDoItem sut)
    {
        sut.MarkComplete();

        sut.IsDone.Should().BeTrue();
        sut.ToString().Should().Contain("Done");
    }
}
