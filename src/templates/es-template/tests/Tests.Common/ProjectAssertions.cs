// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.ES.Template.Tests.Common;

using Nikiforovall.ES.Template.Domain.ProjectAggregate;
using Nikiforovall.ES.Template.Domain.ValueObjects;
using static Nikiforovall.ES.Template.Domain.ProjectAggregate.Events.Events.V1;

public static class ProjectAssertions
{
    public static Project ShouldBeCreatedWith(
        this Project project,
        Guid projectId,
        string name,
        Colour colour)
    {
        project.Id.Should().Be(projectId);
        project.Name.Should().Be(name);
        project.Colour.Should().Be(colour);
        project.Items.Should().BeEmpty();
        project.Status.Should().Be(ProjectStatus.Complete);
        project.Version.Should().Be(1);

        var expectedEvent = ProjectCreated.Create(projectId, name, colour, project.CreatedAt);

        project.PublishedEvent<ProjectCreated>().Should()
            .BeEquivalentTo(expectedEvent);

        return project;
    }

    public static DateTime ShouldHappenRecently(
        this DateTime dateTime,
        DateTime? time = default,
        TimeSpan? precision = default)
    {
        dateTime.Should().BeCloseTo(time ?? DateTime.UtcNow, precision ?? TimeSpan.FromSeconds(1));

        return dateTime;
    }
}
