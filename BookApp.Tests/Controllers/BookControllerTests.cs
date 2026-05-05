using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApp.Controllers;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Tests.Controllers;

/// <summary>
/// Unit tests for BookController CRUD operations.
/// </summary>
public class BookControllerTests
{
    /// <summary>
    /// Creates a fresh in-memory database context for each test.
    /// </summary>
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        return new ApplicationDbContext(options);
    }

    [Fact]
    public async Task Index_ReturnsViewResult_WithListOfBooks()
    {
        // Arrange
        using var context = CreateContext();
        var author = new Author { Name = "Sigrid Undset", Nationality = "Norsk" };
        context.Authors.Add(author);
        context.Books.AddRange(
            new Book { Title = "Kristin Lavransdatter", AuthorId = author.Id, Genre = "Historisk" },
            new Book { Title = "Olav Audunssøn", AuthorId = author.Id, Genre = "Historisk" }
        );
        await context.SaveChangesAsync();

        var controller = new BookController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Book>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Create_Post_AddsBookToDatabase()
    {
        // Arrange
        using var context = CreateContext();
        var author = new Author { Name = "Knut Hamsun", Nationality = "Norsk" };
        context.Authors.Add(author);
        await context.SaveChangesAsync();

        var controller = new BookController(context);
        var newBook = new Book { Title = "Sult", AuthorId = author.Id, Genre = "Roman" };

        // Act
        await controller.Create(newBook);

        // Assert
        Assert.Equal(1, context.Books.Count());
        Assert.Equal("Sult", context.Books.First().Title);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new BookController(context);

        // Act
        var result = await controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesBook_FromDatabase()
    {
        // Arrange
        using var context = CreateContext();
        var author = new Author { Name = "Ibsen", Nationality = "Norsk" };
        context.Authors.Add(author);
        var book = new Book { Title = "Peer Gynt", AuthorId = author.Id };
        context.Books.Add(book);
        await context.SaveChangesAsync();

        var controller = new BookController(context);

        // Act
        await controller.DeleteConfirmed(book.Id);

        // Assert
        Assert.Equal(0, context.Books.Count());
    }
}