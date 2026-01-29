using System.ComponentModel.DataAnnotations;

namespace DesafioApi.Entities;

public class Usuario
{
    public int Id { get; set; }

    [MaxLength(50)]
    public required string NomeUsuario { get; set; }

    [MaxLength(100)]
    public required string Senha { get; set; }

    [MaxLength(100)]
    public string? Email { get; set; }

    public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
}
