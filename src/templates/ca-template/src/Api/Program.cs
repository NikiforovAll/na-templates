// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.


using Nikiforovall.CA.Template.Api;
using Nikiforovall.CA.Template.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;
var applicationName = hostEnvironment.ApplicationName;
var environmentName = hostEnvironment.EnvironmentName;

Log.Logger = ApplicationLoggerFactory
    .CreateLogger(builder.Configuration, hostEnvironment);

builder.Host
    .UseSerilog()
    .UseDefaultServiceProvider((context, options) =>
    {
        var isDevelopment = context.HostingEnvironment.IsDevelopment();
        options.ValidateScopes = isDevelopment;
        options.ValidateOnBuild = isDevelopment;
    });

var startup = new Startup(builder.Configuration, builder.Environment);

startup.ConfigureServices(builder.Services);
var app = builder.Build();
startup.Configure(app);

try
{
    LogLifecycle("Started {Application} in {Environment} mode.");
    await app.RunAsync();
    LogLifecycle("Stopped {Application} in {Environment} mode.");
    return 0;
}
catch (Exception exception)
{
    LogCrash(exception);
    return 1;
}
finally
{
    Log.CloseAndFlush();
}

void LogLifecycle(string msg) => Log.Information(msg, applicationName, environmentName);

void LogCrash(Exception exception) => Log.Fatal(
    exception,
    "{Application} terminated unexpectedly in {Environment} mode.",
    hostEnvironment.ApplicationName,
    hostEnvironment.EnvironmentName);
