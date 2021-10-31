namespace Nikiforoval.Templates.Tests;

using System.IO;
using Boxed.DotnetNewTest;
using Xunit;
using Xunit.Abstractions;

[Trait("Temlate", "CSharp")]
public class EventSourcingTemplateTests
{
    private const string TemplateName = "na-es";
    private const string SolutionFileName = "NikiforovAll.EventSourcing.Template.sln";
    private static readonly string[] DefaultArguments = Array.Empty<string>();

    public EventSourcingTemplateTests(ITestOutputHelper testOutputHelper)
    {
        if (testOutputHelper is null)
        {
            throw new ArgumentNullException(nameof(testOutputHelper));
        }

        TestLogger.WriteMessage = testOutputHelper.WriteLine;
    }

    [Theory]
    [InlineData("demo-es")]
    public async Task DefaultTemplate(string name, params string[] arguments)
    {
        await InstallTemplateAsync().ConfigureAwait(false);
        await using var tempDirectory = TempDirectory.NewTempDirectory();
        var project = await tempDirectory
            .DotnetNewAsync(TemplateName, name, DefaultArguments.ToArguments(arguments))
            .ConfigureAwait(false);

        var apiProject = new Project(
            project.Name,
            Path.Combine(project.DirectoryPath, "src", "Api"),
            project.PublishDirectoryPath,
            project.HttpsPort, project.HttpPort);

        var domainUnitTestsProject = new Project(
            project.Name,
            Path.Combine(project.DirectoryPath, "tests", "Domain.UnitTests"),
            project.PublishDirectoryPath,
            project.HttpsPort, project.HttpPort);

        await apiProject.DotnetRestoreAsync().ConfigureAwait(false);
        await apiProject.DotnetBuildAsync().ConfigureAwait(false);

        await domainUnitTestsProject.DotnetRestoreAsync().ConfigureAwait(false);
        await domainUnitTestsProject.DotnetTestAsync().ConfigureAwait(false);

        Assert.True(File.Exists(Path.Combine(project.DirectoryPath, "README.md")));
    }

    private static Task InstallTemplateAsync() => DotnetNew.InstallAsync<EventSourcingTemplateTests>(SolutionFileName);
}
