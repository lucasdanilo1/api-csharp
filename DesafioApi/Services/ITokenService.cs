using DesafioApi.Entities;

namespace DesafioApi.Services;

public interface ITokenService
{
    string GerarToken(Usuario usuario);
}
