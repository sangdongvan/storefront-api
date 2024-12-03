namespace Storefront.Models;

public class RefreshUserTokenRequest
{
    public required string RefreshToken { get; init; } = string.Empty;
}