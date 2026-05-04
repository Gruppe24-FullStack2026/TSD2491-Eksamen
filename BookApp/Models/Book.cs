using System.ComponentModel.DataAnnotations;

namespace BookApp.Models;

/// <summary>
/// Represents a book in the library system.
/// </summary>
public class Book
{
    public int Id { get; set; }

    /// <summary>
    /// Title of the book.
    /// </summary>
    [Required(ErrorMessage = "Title is required")]
    [StringLength(200, ErrorMessage = "Title cannot be longer than 200 characters")]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// International Standard Book Number.
    /// </summary>
    [StringLength(13, ErrorMessage = "ISBN cannot be longer than 13 characters")]
    public string? Isbn { get; set; }

    /// <summary>
    /// Year the book was published.
    /// </summary>
    [Range(1000, 2100, ErrorMessage = "Year of publication must be between 1000 and 2100")]
    public int? PublishedYear { get; set; }

    /// <summary>
    /// Genre of the book.
    /// </summary>
    [StringLength(100, ErrorMessage = "Genre cannot be longer than 100 characters")]
    public string? Genre { get; set; }

    /// <summary>
    /// Foreign key to the author.
    /// </summary>
    [Required(ErrorMessage = "Author is required")]
    public int AuthorId { get; set; }

    /// <summary>
    /// Navigation property to the book's author.
    /// </summary>
    public Author? Author { get; set; }
}