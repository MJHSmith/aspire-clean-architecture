using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Authentication;

public class JwtOptions
{
    [Required]
    public string Issuer { get; set; } = string.Empty;

    [Required]
    public string Audience { get; set; } = string.Empty;

    [Required]
    public string Secret { get; set; } = string.Empty;

    [Range(1, int.MaxValue)]
    public int ExpirationInMinutes { get; set; }
}

public readonly record struct InfrastructureOptions(JwtOptions? JwtOptions, string? ConnectionString);
