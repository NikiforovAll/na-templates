// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Console.Commands.SeedCommands;

using AutoBogus;
using Nikiforovall.CA.Template.Domain.ProjectAggregate;
using Nikiforovall.CA.Template.Domain.ValueObjects;

public class ProjectFaker : AutoFaker<Project>
{
    public ProjectFaker()
    {
        var todoItemFaker = new ToDoItemFaker();

        var colours = Colour.SupportedColours.ToList();

        this.CustomInstantiator(faker =>
            new Project(this.FakerHub.Company.Random.Word(), faker.PickRandom(colours)));

        this.RuleFor(faker => faker.Colour, faker => faker.PickRandom(colours));

        this.Configure(builder => builder
            .WithSkip<Project>(p => p.LastModified)
            .WithSkip<Project>(p => p.LastModifiedBy));

        this.FinishWith((fake, project) =>
        {
            var todos = todoItemFaker.Generate(fake.Random.Number(3));

            foreach (var t in todos)
            {
                project.AddItem(t);
            }
        });
    }
}
