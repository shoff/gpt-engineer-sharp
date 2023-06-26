namespace GptEngineer.Core.Tests.Infrastructure;

using Base;
using GptEngineer.Core.Projects;
using GptEngineer.Infrastructure.Services;
using Moq;
using Xunit.Abstractions;



public class ProjectServiceTests : BaseTest
{
    public ProjectServiceTests(ITestOutputHelper outputHelper) 
        : base(outputHelper)
    {
    }
    [Fact]
    public async Task GetProjectsAsync_GivenNoProjectDirectories_ReturnsEmpty()
    {
        // Arrange
        var mockFileSystem = new Mock<IFileSystem>();
        mockFileSystem.Setup(fs => fs.GetDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EnumerationOptions>()))
            .Returns(new string[0]);

        var projectFactory = new ProjectFactory(mockFileSystem.Object);

        // Act
        var projects = await projectFactory.GetProjectsAsync();

        // Assert
        Assert.Empty(projects);
    }

    [Fact]
    public async Task GetProjectsAsync_GivenOneProjectDirectory_ReturnsOneProject()
    {
        // Arrange
        var mockFileSystem = new Mock<IFileSystem>();
        mockFileSystem.Setup(fs => fs.GetDirectories(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<EnumerationOptions>()))
            .Returns(new[] { "project1" });

        var projectFactory = new ProjectFactory(mockFileSystem.Object);

        // Act
        var projects = await projectFactory.GetProjectsAsync();

        // Assert
        Assert.Single(projects);
    }

}