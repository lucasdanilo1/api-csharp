using DesafioApi.Entities;
using Microsoft.EntityFrameworkCore;

namespace DesafioApi.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Usuario> Usuarios { get; set; }
}
