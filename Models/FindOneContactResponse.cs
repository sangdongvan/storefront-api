namespace Storefront.Models;

public class FindOneContactResponse
{
    public string Id { get; init; } = string.Empty;
    public string Avatar { get; init; } = string.Empty;
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public string Twitter { get; init; } = string.Empty;
    public string Notes { get; init; } = string.Empty;
    public bool Favorite { get; init; }
}