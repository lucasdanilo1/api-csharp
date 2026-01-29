using DesafioApi.Entities;
using DesafioApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DesafioApi.Data;

public static class DatabaseInitializer
{
    public static void Initialize(AppDbContext context, AdminPadraoSettings adminSettings)
    {
        // AVISO: EnsureDeleted/EnsureCreated são para desenvolvimento/testes apenas
        // Para produção, use Migrations: dotnet ef migrations add InitialCreate && dotnet ef database update

        // Remove o banco e recria (apenas para desenvolvimento/testes)
        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        // Verifica se já existe usuário admin
        if (context.Usuarios.Any())
        {
            return; // Banco já possui dados
        }

        // Cria usuário admin padrão
        var adminUser = new Usuario
        {
            NomeUsuario = adminSettings.NomeUsuario,
            Email = adminSettings.Email,
            Senha = adminSettings.Senha
        };

        context.Usuarios.Add(adminUser);
        context.SaveChanges();
    }
}
