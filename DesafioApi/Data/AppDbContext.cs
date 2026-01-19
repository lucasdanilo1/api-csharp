using DesafioApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>().HasData(new Usuario
        {
            Id = 1,
            NomeUsuario = "marcus123",
            Email = "admin@montreal.com",
            Senha = "$2a$12$A62hByjiTNQHuuZF3DokqOcOa1cYIqzL9mQz486XlekRo9f8xgruW" // senha123
        });
    }
}
