using DesafioApi.Dtos.Request;
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
    public async Task<IActionResult> LerArquivo([FromBody] CaminhoArquivoRequest dto)
    {
        var resultado = await arquivoService.LerArquivoAsync(dto.CaminhoArquivo);
        return Ok(resultado);
    }

    [HttpPut("atualizar")]
    [Authorize]
    public async Task<IActionResult> AtualizarArquivo([FromBody] AtualizarArquivoRequest dto)
    {
        var resultado = await arquivoService.AtualizarArquivoAsync(dto.CaminhoArquivo, dto.NovoConteudo);
        return Ok(resultado);
    }
}
