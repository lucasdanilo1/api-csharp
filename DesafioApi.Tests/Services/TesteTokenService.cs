using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DesafioApi.Entities;
using DesafioApi.Services;
using DesafioApi.Settings;
using Microsoft.Extensions.Options;
using Xunit;

namespace DesafioApi.Tests.Services;

public class TesteTokenService
{
    private readonly TokenService _servico;
    private readonly JwtSettings _jwtSettings;

    public TesteTokenService()
    {
        _jwtSettings = new JwtSettings
        {
            SecretKey = "ChaveSecretaMuitoLongaParaTestesDe256Bits!@#$%",
            Issuer = "TesteIssuer",
            Audience = "TesteAudience",
            MinutosExpiracao = 60
        };

        var options = Options.Create(_jwtSettings);
        _servico = new TokenService(options);
    }

    [Fact]
    public void GerarToken_ComUsuarioValido_RetornaTokenJwt()
    {
        var usuario = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var token = _servico.GerarToken(usuario);

        Assert.NotNull(token);
        Assert.NotEmpty(token);
    }

    [Fact]
    public void GerarToken_TokenContemClaimNome()
    {
        var usuario = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var token = _servico.GerarToken(usuario);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claimNome = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name);

        Assert.NotNull(claimNome);
        Assert.Equal("usuario_teste", claimNome.Value);
    }

    [Fact]
    public void GerarToken_TokenContemClaimId()
    {
        var usuario = new Usuario
        {
            Id = 42,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var token = _servico.GerarToken(usuario);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var claimId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);

        Assert.NotNull(claimId);
        Assert.Equal("42", claimId.Value);
    }

    [Fact]
    public void GerarToken_TokenContemJti()
    {
        var usuario = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var token = _servico.GerarToken(usuario);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var jtiClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Jti);

        Assert.NotNull(jtiClaim);
        Assert.True(Guid.TryParse(jtiClaim.Value, out _));
    }

    [Fact]
    public void GerarToken_TokenContemIssuerCorreto()
    {
        var usuario = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var token = _servico.GerarToken(usuario);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Equal(_jwtSettings.Issuer, jwtToken.Issuer);
    }

    [Fact]
    public void GerarToken_TokenContemAudienceCorreto()
    {
        var usuario = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var token = _servico.GerarToken(usuario);
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        Assert.Contains(_jwtSettings.Audience, jwtToken.Audiences);
    }

    [Fact]
    public void GerarToken_TokenTemTempoExpiracaoCorreto()
    {
        var usuario = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var antesDaGeracao = DateTime.UtcNow;
        var token = _servico.GerarToken(usuario);
        var depoisDaGeracao = DateTime.UtcNow;

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        var esperadoMinimo = antesDaGeracao.AddMinutes(_jwtSettings.MinutosExpiracao);
        var esperadoMaximo = depoisDaGeracao.AddMinutes(_jwtSettings.MinutosExpiracao).AddSeconds(1);

        Assert.True(jwtToken.ValidTo >= esperadoMinimo.AddSeconds(-1));
        Assert.True(jwtToken.ValidTo <= esperadoMaximo);
    }

    [Fact]
    public void GerarToken_TokensConsecutivosSaoDiferentes()
    {
        var usuario = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_teste",
            Senha = "senha_hash"
        };

        var token1 = _servico.GerarToken(usuario);
        var token2 = _servico.GerarToken(usuario);

        Assert.NotEqual(token1, token2);
    }

    [Fact]
    public void GerarToken_ComUsuariosDiferentes_RetornaTokensDiferentes()
    {
        var usuario1 = new Usuario
        {
            Id = 1,
            NomeUsuario = "usuario_1",
            Senha = "senha_hash"
        };

        var usuario2 = new Usuario
        {
            Id = 2,
            NomeUsuario = "usuario_2",
            Senha = "senha_hash"
        };

        var token1 = _servico.GerarToken(usuario1);
        var token2 = _servico.GerarToken(usuario2);

        Assert.NotEqual(token1, token2);
    }
}
