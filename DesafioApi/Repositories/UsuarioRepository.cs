using DesafioApi.Data;
using DesafioApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioApi.Repositories;

public class UsuarioRepository(AppDbContext context) : IUsuarioRepository
{
    public async Task<Usuario?> FindByNomeUsuarioAsync(string nomeUsuario)
        => await context.Usuarios.FirstOrDefaultAsync(u => u.NomeUsuario == nomeUsuario);
}
