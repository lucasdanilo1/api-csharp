using DesafioApi.Entities;
using DesafioApi.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DesafioApi.Data;

public static class DatabaseInitializer
{
    public static void Initialize(AppDbContext context, AdminPadraoSettings adminSettings)
    {

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        if (context.Usuarios.Any())
        {
            return; 
        }

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
