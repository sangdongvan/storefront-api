namespace Storefront.Models;

public class GetUserTokenRequest
{
    public required string Email { get; init; } = string.Empty;
    public required string Password { get; init; } = string.Empty;
    public string? TwoFactorCode { get; init; }
    public string? TwoFactorRecoveryCode { get; init; }
}