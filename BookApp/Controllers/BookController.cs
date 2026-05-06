using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Controllers;

/// <summary>
/// Controller for managing books in the library system.
/// </summary>
public class BookController : Controller
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of BookController.
    /// </summary>
    /// <param name="context">The database context.</param>
    public BookController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Displays a list of all books with their authors.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        var books = await _context.Books.Include(b => b.Author).ToListAsync();
        return View(books);
    }

    /// <summary>
    /// Displays details for a specific book.
    /// </summary>
    /// <param name="id">The unique identifier of the book.</param>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null) return NotFound();

        return View(book);
    }

    /// <summary>
    /// Displays the form for creating a new book.
    /// </summary>
    public IActionResult Create()
    {
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name");
        return View();
    }

    /// <summary>
    /// Handles the form submission for creating a new book.
    /// </summary>
    /// <param name="book">The book object submitted from the form.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,Isbn,PublishedYear,Genre,AuthorId")] Book book)
    {
        if (ModelState.IsValid)
        {
            _context.Add(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
        return View(book);
    }

    /// <summary>
    /// Displays the form for editing an existing book.
    /// </summary>
    /// <param name="id">The unique identifier of the book to edit.</param>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books.FindAsync(id);

        if (book == null) return NotFound();

        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
        return View(book);
    }

    /// <summary>
    /// Handles the form submission for editing an existing book.
    /// </summary>
    /// <param name="id">The unique identifier of the book.</param>
    /// <param name="book">The updated book object submitted from the form.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Isbn,PublishedYear,Genre,AuthorId")] Book book)
    {
        if (id != book.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(book);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!BookExists(book.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Name", book.AuthorId);
        return View(book);
    }

    /// <summary>
    /// Displays the confirmation page for deleting a book.
    /// </summary>
    /// <param name="id">The unique identifier of the book to delete.</param>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var book = await _context.Books
            .Include(b => b.Author)
            .FirstOrDefaultAsync(m => m.Id == id);

        if (book == null) return NotFound();

        return View(book);
    }

    /// <summary>
    /// Handles the confirmed deletion of a book.
    /// </summary>
    /// <param name="id">The unique identifier of the book to delete.</param>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book != null) _context.Books.Remove(book);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Checks whether a book with the given ID exists in the database.
    /// </summary>
    /// <param name="id">The unique identifier of the book.</param>
    private bool BookExists(int id)
    {
        return _context.Books.Any(e => e.Id == id);
    }

    /// <summary>
    /// Searches for books by title or author name.
    /// </summary>
    /// <param name="query">The search string to filter by.</param>
    public async Task<IActionResult> Search(string query)
    {
        if (string.IsNullOrWhiteSpace(query))
            return View("Index", await _context.Books.Include(b => b.Author).ToListAsync());

        var results = await _context.Books
            .Include(b => b.Author)
            .Where(b => b.Title.Contains(query) || b.Author!.Name.Contains(query))
            .ToListAsync();

        return View("Index", results);
    }
}