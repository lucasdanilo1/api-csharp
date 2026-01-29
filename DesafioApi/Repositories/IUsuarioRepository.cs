using DesafioApi.Entities;

namespace DesafioApi.Repositories;

public interface IUsuarioRepository
{
    Task<Usuario?> FindByNomeUsuarioAsync(string nomeUsuario);
}
