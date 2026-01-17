using System.Security;
using DesafioApi.Dtos;

namespace DesafioApi.Services;

public class FileService : IFileService
{
    private readonly string _baseDirectory;

    public FileService(IConfiguration configuration)
    {
        _baseDirectory = configuration["AllowedDirectory"] ?? Environment.CurrentDirectory;
    }

    public async Task<FileReadResponseDto> ReadAsync(string path, CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(path))
        {
            throw new ArgumentException("Path cannot be null or empty.", nameof(path));
        }

        var fullPath = Path.GetFullPath(Path.Combine(_baseDirectory, path));

        if (!fullPath.StartsWith(_baseDirectory, StringComparison.OrdinalIgnoreCase))
        {
            throw new UnauthorizedAccessException("Access denied: path traversal attempt.");
        }

        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException("File not found.", path);
        }

        try
        {
            var fileName = Path.GetFileName(fullPath);
            var content = await File.ReadAllTextAsync(fullPath, ct);
            return new FileReadResponseDto(fileName, content);
        }
        catch (Exception ex) when (ex is IOException or SecurityException or UnauthorizedAccessException)
        {
            throw new InvalidOperationException("Error reading file.", ex);
        }
    }
}
