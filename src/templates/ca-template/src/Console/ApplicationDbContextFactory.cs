// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace Nikiforovall.CA.Template.Console;

using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nikiforovall.CA.Template.Application;
using Nikiforovall.CA.Template.Application.SharedKernel.Interfaces;
using Nikiforovall.CA.Template.Infrastructure;
using Nikiforovall.CA.Template.Infrastructure.Persistence;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();

        var services = new ServiceCollection();
        services.AddLogging();
        services.AddApplication();
        services.AddInfrastructure(configuration);
        services.AddSingleton<ICurrentUserService, StubUserService>();

        var scope = services.BuildServiceProvider();

        return scope.GetRequiredService<ApplicationDbContext>();
    }
}

public class StubUserService : ICurrentUserService
{
    public string? UserId => default;
}
