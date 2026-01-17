using DesafioApi.Dtos;

namespace DesafioApi.Services;

public interface IFileService
{
    Task<FileReadResponseDto> ReadAsync(string path, CancellationToken ct);
}
