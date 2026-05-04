// Models/Book.cs
using System.ComponentModel.DataAnnotations;

namespace BookApp.Models
{
    /// <summary>
    /// Represents a book available in a Norwegian library system.
    /// </summary>
    public class Book
    {
        /// <summary>
        /// Primary key for the book entry.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the book.
        /// </summary>
        [Required(ErrorMessage = "Title is required.")]
        [StringLength(200, MinimumLength = 1, ErrorMessage = "Title must be between 1 and 200 characters.")]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Name of the author.
        /// </summary>
        [Required(ErrorMessage = "Author is required.")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Author name must be between 2 and 100 characters.")]
        [Display(Name = "Author")]
        public string Author { get; set; } = string.Empty;

        /// <summary>
        /// International Standard Book Number.
        /// </summary>
        [StringLength(13, MinimumLength = 10, ErrorMessage = "ISBN must be between 10 and 13 characters.")]
        [Display(Name = "ISBN")]
        public string? Isbn { get; set; }

        /// <summary>
        /// Year the book was published.
        /// </summary>
        [Range(1000, 2100, ErrorMessage = "Publication year must be between 1000 and 2100.")]
        [Display(Name = "Publication Year")]
        public int? PublicationYear { get; set; }

        /// <summary>
        /// Publisher of the book.
        /// </summary>
        [StringLength(150, ErrorMessage = "Publisher name cannot exceed 150 characters.")]
        [Display(Name = "Publisher")]
        public string? Publisher { get; set; }

        /// <summary>
        /// Language the book is written in.
        /// </summary>
        [StringLength(50, ErrorMessage = "Language cannot exceed 50 characters.")]
        [Display(Name = "Language")]
        public string? Language { get; set; }

        /// <summary>
        /// Short description or summary of the book.
        /// </summary>
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters.")]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        /// <summary>
        /// Whether the book is currently available for loan.
        /// </summary>
        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;
    }
}