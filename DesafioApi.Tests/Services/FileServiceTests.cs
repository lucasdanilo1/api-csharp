using DesafioApi.Services;
using Moq;
using System.IO;
using Xunit;

namespace DesafioApi.Tests.Services;

public class FileServiceTests
{
    private readonly Mock<IConfiguration> _mockConfiguration;

    public FileServiceTests()
    {
        _mockConfiguration = new Mock<IConfiguration>();
    }

    [Fact]
    public async Task ReadAsync_WhenPathIsNullOrEmpty_ThrowsArgumentException()
    {
        _mockConfiguration.Setup(c => c["AllowedDirectory"]).Returns("C:\\AllowedFiles");
        var service = new FileService(_mockConfiguration.Object);

        await Assert.ThrowsAsync<ArgumentException>(() => service.ReadAsync("", CancellationToken.None));
    }

    [Fact]
    public async Task ReadAsync_WhenFileDoesNotExist_ThrowsFileNotFoundException()
    {
        _mockConfiguration.Setup(c => c["AllowedDirectory"]).Returns("C:\\AllowedFiles");
        var service = new FileService(_mockConfiguration.Object);

        await Assert.ThrowsAsync<FileNotFoundException>(() => service.ReadAsync("nonexistent.txt", CancellationToken.None));
    }

    [Fact]
    public async Task ReadAsync_WhenPathTraversalAttempted_ThrowsUnauthorizedAccessException()
    {
        _mockConfiguration.Setup(c => c["AllowedDirectory"]).Returns("C:\\AllowedFiles");
        var service = new FileService(_mockConfiguration.Object);

        await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.ReadAsync("..\\..\\windows\\system32\\config", CancellationToken.None));
    }

    [Fact]
    public async Task ReadAsync_WhenFileExists_ReturnsFileContent()
    {
        var testDirectory = Path.Combine(Path.GetTempPath(), "TestFiles");
        Directory.CreateDirectory(testDirectory);
        
        var testFileName = "test.txt";
        var testContent = "Hello, World!";
        var testFilePath = Path.Combine(testDirectory, testFileName);
        
        await File.WriteAllTextAsync(testFilePath, testContent);

        _mockConfiguration.Setup(c => c["AllowedDirectory"]).Returns(testDirectory);
        var service = new FileService(_mockConfiguration.Object);

        var result = await service.ReadAsync(testFileName, CancellationToken.None);

        Assert.Equal(testFileName, result.FileName);
        Assert.Equal(testContent, result.Content);

        Directory.Delete(testDirectory, true);
    }
}
