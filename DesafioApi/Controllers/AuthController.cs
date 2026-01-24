using DesafioApi.Dtos.Request;
using DesafioApi.Dtos.Response;
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
    public async Task<IActionResult> Login([FromBody] LoginRequest requisicao)
    {
        var usuario = await usuarioRepository.FindByNomeUsuarioAsync(requisicao.NomeUsuario);

        if (usuario is null) return Unauthorized();

        if (!BCrypt.Net.BCrypt.Verify(requisicao.Senha, usuario.Senha)) return Unauthorized();

        var token = tokenService.GerarToken(usuario);
        return Ok(new LoginResponse(token));
    }
}
