using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Controllers;

/// <summary>
/// Controller for managing authors in the library system.
/// </summary>
public class AuthorController : Controller
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Initializes a new instance of AuthorController.
    /// </summary>
    /// <param name="context">The database context.</param>
    public AuthorController(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Displays a list of all authors.
    /// </summary>
    public async Task<IActionResult> Index()
    {
        return View(await _context.Authors.ToListAsync());
    }

    /// <summary>
    /// Displays details for a specific author.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);

        if (author == null) return NotFound();

        return View(author);
    }

    /// <summary>
    /// Displays the form for creating a new author.
    /// </summary>
    public IActionResult Create()
    {
        return View();
    }

    /// <summary>
    /// Handles the form submission for creating a new author.
    /// </summary>
    /// <param name="author">The author object submitted from the form.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Name,Nationality")] Author author)
    {
        if (ModelState.IsValid)
        {
            _context.Add(author);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    /// <summary>
    /// Displays the form for editing an existing author.
    /// </summary>
    /// <param name="id">The unique identifier of the author to edit.</param>
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null) return NotFound();

        var author = await _context.Authors.FindAsync(id);

        if (author == null) return NotFound();

        return View(author);
    }

    /// <summary>
    /// Handles the form submission for editing an existing author.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    /// <param name="author">The updated author object submitted from the form.</param>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Nationality")] Author author)
    {
        if (id != author.Id) return NotFound();

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(author);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AuthorExists(author.Id)) return NotFound();
                else throw;
            }
            return RedirectToAction(nameof(Index));
        }
        return View(author);
    }

    /// <summary>
    /// Displays the confirmation page for deleting an author.
    /// </summary>
    /// <param name="id">The unique identifier of the author to delete.</param>
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null) return NotFound();

        var author = await _context.Authors
            .FirstOrDefaultAsync(m => m.Id == id);

        if (author == null) return NotFound();

        return View(author);
    }

    /// <summary>
    /// Handles the confirmed deletion of an author.
    /// </summary>
    /// <param name="id">The unique identifier of the author to delete.</param>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author != null) _context.Authors.Remove(author);

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /// <summary>
    /// Checks whether an author with the given ID exists in the database.
    /// </summary>
    /// <param name="id">The unique identifier of the author.</param>
    private bool AuthorExists(int id)
    {
        return _context.Authors.Any(e => e.Id == id);
    }
}