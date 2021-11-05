namespace Nikiforoval.Templates.Tests;

using System.IO;
using Boxed.DotnetNewTest;
using Xunit;
using Xunit.Abstractions;

[Trait("Temlate", "CSharp")]
public class GatewayProjectTemplateTests
{
    private const string TemplateName = "na-ga";
    private const string SolutionFileName = "NikiforovAll.Gateway.Template.sln";
    private static readonly string[] DefaultArguments = Array.Empty<string>();

    public GatewayProjectTemplateTests(ITestOutputHelper testOutputHelper)
    {
        if (testOutputHelper is null)
        {
            throw new ArgumentNullException(nameof(testOutputHelper));
        }

        TestLogger.WriteMessage = testOutputHelper.WriteLine;
    }

    [Theory]
    [InlineData("demo-ga")]
    public async Task DefaultTemplate(string name, params string[] arguments)
    {
        await InstallTemplateAsync().ConfigureAwait(false);
        await using var tempDirectory = TempDirectory.NewTempDirectory();
        var project = await tempDirectory
            .DotnetNewAsync(TemplateName, name, DefaultArguments.ToArguments(arguments))
            .ConfigureAwait(false);

        Assert.True(File.Exists(Path.Combine(project.DirectoryPath, "README.md")));
        Assert.True(File.Exists(Path.Combine(project.DirectoryPath, SolutionFileName)));
    }

    private static Task InstallTemplateAsync() => DotnetNew.InstallAsync<EventSourcingTemplateTests>(SolutionFileName);
}
