using System.Security.Claims;
using DesafioApi.Dtos;
using DesafioApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileService _fileService;

    public FileController(IFileService fileService)
    {
        _fileService = fileService;
    }

    [HttpPost("read")]
    [Authorize]
    public async Task<IActionResult> ReadFile([FromBody] FileReadRequestDto request, CancellationToken ct)
    {
        try
        {
            var result = await _fileService.ReadAsync(request.Path, ct);
            return Ok(result);
        }
        catch (ArgumentException)
        {
            return BadRequest(new { message = "Invalid path provided." });
        }
        catch (UnauthorizedAccessException)
        {
            return StatusCode(StatusCodes.Status403Forbidden, new { message = "Access denied." });
        }
        catch (FileNotFoundException)
        {
            return NotFound(new { message = "File not found." });
        }
        catch (InvalidOperationException)
        {
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error reading file." });
        }
    }
}
