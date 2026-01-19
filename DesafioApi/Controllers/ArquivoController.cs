using DesafioApi.Dtos;
using DesafioApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DesafioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArquivoController(IArquivoService arquivoService) : ControllerBase
{
    [HttpGet("ler")]
    [Authorize]
    public async Task<IActionResult> LerArquivo([FromBody] CaminhoArquivoDto dto)
    {
        var resultado = await arquivoService.LerArquivoAsync(dto.CaminhoArquivo);
        return Ok(resultado);
    }
}
