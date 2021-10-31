// Copyright (c) Oleksii Nikiforov, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Domain.UnitTests;

using AutoFixture.Xunit2;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;
using Nikiforovall.CA.Template.Domain.ProjectAggregate.Events;

public class ToDoItemTests
{
    [AutoData, Theory]
    public void ToDoItemConstructed_Sucessfully(string title, string description)
    {
        var sut = new ToDoItem()
        {
            Description = description,
            Title = title,
        };

        sut.Title.Should().Be(title);
        sut.Description.Should().Be(description);
        sut.IsDone.Should().BeFalse();
    }

    [AutoData, Theory]
    public void MarkComplete_CompletedAdnEventDispatched(ToDoItem sut)
    {
        sut.MarkComplete();

        sut.IsDone.Should().BeTrue();
        sut.DomainEvents.Should().ContainSingle()
            .Which.GetType().Name.Should().Be(nameof(ToDoItemCompletedEvent));
        sut.ToString().Should().Contain("Done");
    }
}
