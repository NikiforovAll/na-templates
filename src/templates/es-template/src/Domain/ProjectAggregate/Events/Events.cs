// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace NikiforovAll.ES.Template.Domain.ProjectAggregate.Events;

using NikiforovAll.ES.Template.Domain.SharedKernel.Events;

public static class Events
{
    public static class V1
    {
        public record ProjectCreated : IEvent
        {
            public Guid Id { get; init; }

            public string Name { get; init; } = default!;

            public string Colour { get; init; } = default!;

            public DateTime CreatedAt { get; init; } = default!;

            private ProjectCreated() { }

            public static ProjectCreated Create(
                Guid id,
                string name,
                string colour,
                DateTime createdAt) => new()
                {
                    Id = id,
                    Name = name,
                    Colour = colour,
                    CreatedAt = createdAt,
                };
        }

        public record NewItemAdded : IEvent
        {
            public Guid ItemId { get; init; }

            public int ProjectNumber { get; init; }

            public string Title { get; init; } = string.Empty;

            public string? Description { get; init; }

            public Guid ProjectId { get; init; }

            private NewItemAdded() { }

            public static NewItemAdded Create(
                Guid id, int projectNumber, string title, string? description, Guid projectId) => new()
                {
                    ItemId = id,
                    ProjectNumber = projectNumber,
                    Title = title,
                    Description = description,
                    ProjectId = projectId,
                };
        }

        public record NameUpdated : IEvent
        {
            public string Name { get; init; } = default!;

            private NameUpdated() { }

            public static NameUpdated Create(string name) => new()
            {
                Name = name,
            };
        }

        public record ToDoItemCompleted : IEvent
        {
            public Guid ProjectId { get; set; }

            public int ItemId { get; set; }

            private ToDoItemCompleted() { }

            public static ToDoItemCompleted Create(Guid projectId, int itemId) => new()
            {
                ItemId = itemId,
                ProjectId = projectId,
            };
        }

        public record ProjectArchieved : IEvent
        {
            public Guid ProjectId { get; set; }

            public DateTime DeletedAt { get; set; }

            private ProjectArchieved() { }

            public static ProjectArchieved Create(Guid projectId, DateTime deletedAt) => new()
            {
                ProjectId = projectId,
                DeletedAt = deletedAt,
            };
        }
    }
}
