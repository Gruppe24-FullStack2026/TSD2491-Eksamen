using Microsoft.AspNetCore.Mvc;
using BookApp.Data;
using BookApp.Models;
using BookApp.Services;

namespace BookApp.Controllers;

/// <summary>
/// Controller for importing books from the National Library of Norway API.
/// </summary>
public class ApiImportController : Controller
{
    private readonly BookApiService _apiService;
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of ApiImportController.
    /// </summary>
    /// <param name="apiService">Service for fetching books from the API.</param>
    /// <param name="context">The database context.</param>
    public ApiImportController(BookApiService apiService, ApplicationDbContext context)
    {
        _apiService = apiService;
        _context = context;
    }

    /// <summary>
    /// Displays the search form for importing books from the API.
    /// </summary>
    public IActionResult Index()
    {
        return View();
    }

    /// <summary>
    /// Searches for books from the National Library API based on a query string.
    /// </summary>
    /// <param name="query">The search term to use.</param>
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return View("Index");

        var books = await _apiService.SearchBooksAsync(query);
        ViewData["Query"] = query;
        return View(books);
    }

    /// <summary>
    /// Imports a specific book from the API and saves it to the database.
    /// </summary>
    /// <param name="title">Title of the book to import.</param>
    /// <param name="year">Publication year of the book.</param>
    [HttpPost]
    public async Task<IActionResult> Import(string title, string year, string authorName, string genre)
    {
        var author = _context.Authors.FirstOrDefault(a => a.Name == authorName);

        if (author == null)
        {
            author = new Author { Name = authorName ?? "Ukjent forfatter" };
            _context.Authors.Add(author);
            await _context.SaveChangesAsync();
        }

        var book = new Book
        {
            Title = title,
            PublishedYear = int.TryParse(year, out var y) ? y : null,
            AuthorId = author.Id,
            Genre = genre
        };

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return RedirectToAction("Index", "Book");
    }
}