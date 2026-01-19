using DesafioApi.Dtos;
using DesafioApi.Repositories;
using DesafioApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace DesafioApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(
    IUsuarioRepository usuarioRepository,
    ITokenService tokenService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto requisicao)
    {
        var usuario = await usuarioRepository.FindByNomeUsuarioAsync(requisicao.NomeUsuario);
        
        if (usuario is null) return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(requisicao.Senha, usuario.Senha)) return Unauthorized();

        var token = tokenService.GerarToken(usuario);
        return Ok(new RespostaLoginDto(token));
    }
}
