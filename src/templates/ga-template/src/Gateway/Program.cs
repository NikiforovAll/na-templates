// Copyright (c) Oleksii Nikiforov, 2021. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using NikiforovAll.GA.Template.Gateway;
using Serilog;

var builder = WebApplication.CreateBuilder(args);
var hostEnvironment = builder.Environment;
var applicationName = hostEnvironment.ApplicationName;
var environmentName = hostEnvironment.EnvironmentName;

builder.Host.ConfigureAppConfiguration(
    cfg => cfg
        .AddJsonFile("routes.conf.json"));

Log.Logger = ApplicationLoggerFactory
    .CreateLogger(builder.Configuration, hostEnvironment);
builder.Host.UseSerilog();

builder.Services.AddReverseProxy()
    .LoadFromConfig(builder
        .Configuration
        .GetSection("ReverseProxy"));

var app = builder.Build();
app.MapReverseProxy();

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

