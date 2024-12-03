namespace Storefront.Models;

public class UpdateOneContactRequest
{
    public required string Id { get; init; } = string.Empty;
    public string? Avatar { get; init; }
    public string? FirstName { get; init; }
    public string? LastName { get; init; }
    public string? Twitter { get; init; }
    public string? Notes { get; init; }
    public bool? Favorite { get; init; }
}