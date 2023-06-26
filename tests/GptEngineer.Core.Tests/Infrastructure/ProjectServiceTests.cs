namespace GptEngineer.Core.Tests.Infrastructure;

using Base;
using Configuration;
using GptEngineer.Infrastructure.Projects;
using GptEngineer.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit.Abstractions;


// TODO this test is worthless, it doesn't test anything.
public class ProjectServiceTests : BaseTest
{
    private readonly Mock<ILogger<ProjectFactory>> logger;
    private readonly Mock<IOptions<AIOptions>> options;
    private readonly AIOptions aiOptions;

    public ProjectServiceTests(ITestOutputHelper outputHelper) 
        : base(outputHelper)
    {
        this.aiOptions = new AIOptions
        {
            ProjectPath = "C:\\Projects\\"
        };
        this.options = new Mock<IOptions<AIOptions>>();
        this.options.SetupGet(v => v.Value).Returns(this.aiOptions);
    }

    [Fact]
    public async Task GetProjectsAsync_GivenNoProjectDirectories_ReturnsEmpty()
    {
        // Arrange
        var mockFileSystem = new Mock<IFileSystem>();
        mockFileSystem.Setup(fs => fs.GetDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EnumerationOptions>()))
            .Returns(Array.Empty<string>());

        var projectFactory = new ProjectFactory(
            this.logger.Object,
            this.options.Object);

        // Act
        var projects = await projectFactory.CreateProjectAsync("PATH");

        // Assert
        Assert.NotNull(projects);
    }

    [Fact]
    public async Task GetProjectsAsync_GivenOneProjectDirectory_ReturnsOneProject()
    {
        // Arrange
        var mockFileSystem = new Mock<IFileSystem>();
        mockFileSystem.Setup(fs => fs.GetDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EnumerationOptions>()))
            .Returns(new[] { "project1" });

        var projectFactory = new ProjectFactory(
            this.logger.Object,
            this.options.Object);

        // Act
        var projects = await projectFactory.CreateProjectAsync("PATH");

        // Assert
        Assert.NotNull(projects);
    }

}