using System.ComponentModel.DataAnnotations;

namespace BookApp.Models;

/// <summary>
/// Represents a public library that holds books.
/// </summary>
public class Library
{
    public int Id { get; set; }

    /// <summary>
    /// Name of the library.
    /// </summary>
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, ErrorMessage = "Name cannot be longer than 200 characters")]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Municipality the library is located in.
    /// </summary>
    [Required(ErrorMessage = "Municipality is requied")]
    [StringLength(100, ErrorMessage = "Municipality cannot be longer than 100 characters")]
    public string Municipality { get; set; } = string.Empty;
}
