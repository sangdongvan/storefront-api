namespace Storefront.Models;

public class MarkAsFavoriteContactRequest
{
    public required string Id { get; set; } = string.Empty;
    public bool Favorite { get; set; }
}