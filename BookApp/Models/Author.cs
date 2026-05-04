using System.ComponentModel.DataAnnotations;

namespace BookApp.Models;

/// <summary>
/// Represents an author of one or more books.
/// </summary>
public class Author
{
    public int Id { get; set; }

    /// <summary>
    /// Full name of the author.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(100, ErrorMessage = "Name cannot be longer than 100 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Country of origin for the author.
    /// </summary>
    [StringLength(100, ErrorMessage = "Nationality cannot be longer than 100 characters")]
    public string? Nationality { get; set; }

    /// <summary>
    /// Navigation property to the author's books.
    /// </summary>
    public ICollection<Book> Books { get; set; } = new List<Book>();
}