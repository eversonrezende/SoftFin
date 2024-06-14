namespace SoftFin.Core.Models;

public class Category
{
    // The Id is long because each User has create many categories
    public long Id { get; set; }
    // Initialize the Title and Description to empty string to avoid null reference exception
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    // The UserId references the User that created the category
    public string UserId { get; set; } = string.Empty;
}
