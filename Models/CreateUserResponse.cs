namespace Storefront.Models;

public class CreateUserResponse
{
    public required string Id { get; init; } = string.Empty;
    public required string Email { get; init; } = string.Empty;
    public GetUserTokenResponse Token { get; init; } = default!;
}