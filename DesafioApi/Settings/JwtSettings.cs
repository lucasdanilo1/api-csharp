namespace DesafioApi.Settings;

public class JwtSettings
{
    public required string SecretKey { get; init; }
    public required string Issuer { get; init; }
    public required string Audience { get; init; }
    public int MinutosExpiracao { get; init; } = 60;
}
