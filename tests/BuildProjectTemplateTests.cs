namespace Nikiforoval.Templates.Tests;

using System.IO;
using Boxed.DotnetNewTest;
using Xunit;
using Xunit.Abstractions;

[Trait("Temlate", "CSharp")]
public class BuildProjectTemplateTests
{
    private const string TemplateName = "na-bu";
    private const string SolutionFileName = "NikiforovAll.BuildProject.Template.sln";
    private static readonly string[] DefaultArguments = Array.Empty<string>();

    public BuildProjectTemplateTests(ITestOutputHelper testOutputHelper)
    {
        if (testOutputHelper is null)
        {
            throw new ArgumentNullException(nameof(testOutputHelper));
        }

        TestLogger.WriteMessage = testOutputHelper.WriteLine;
    }

    [Theory]
    [InlineData("demo-bu")]
    public async Task DefaultTemplate(string name, params string[] arguments)
    {
        await InstallTemplateAsync().ConfigureAwait(false);
        await using var tempDirectory = TempDirectory.NewTempDirectory();
        var project = await tempDirectory
            .DotnetNewAsync(TemplateName, name, DefaultArguments.ToArguments(arguments))
            .ConfigureAwait(false);

        Assert.True(File.Exists(Path.Combine(project.DirectoryPath, "README.md")));
        Assert.True(File.Exists(Path.Combine(project.DirectoryPath, "docker-compose.yml")));
        Assert.False(File.Exists(Path.Combine(project.DirectoryPath, SolutionFileName)));
    }

    private static Task InstallTemplateAsync() => DotnetNew.InstallAsync<EventSourcingTemplateTests>(SolutionFileName);
}
