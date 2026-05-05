using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BookApp.Models;

namespace BookApp.Data;

/// <summary>
/// Database context for the BookApp application with Identity support.
/// </summary>
public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    /// <summary>
    /// Initializes a new instance of ApplicationDbContext.
    /// </summary>
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    /// <summary>
    /// Database table for books.
    /// </summary>
    public DbSet<Book> Books { get; set; }

    /// <summary>
    /// Database table for authors.
    /// </summary>
    public DbSet<Author> Authors { get; set; }

    /// <summary>
    /// Database table for libraries.
    /// </summary>
    public DbSet<Library> Libraries { get; set; }
}