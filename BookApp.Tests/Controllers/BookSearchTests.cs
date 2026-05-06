using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApp.Controllers;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Tests.Controllers;

/// <summary>
/// Unit tests for BookController search and filtering functionality.
/// </summary>
public class BookSearchTests
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
    public async Task Search_ReturnsMatchingBooks_ByTitle()
    {
        // Arrange
        using var context = CreateContext();
        var author = new Author { Name = "Knut Hamsun", Nationality = "Norsk" };
        context.Authors.Add(author);
        context.Books.AddRange(
            new Book { Title = "Sult", AuthorId = author.Id, Genre = "Roman" },
            new Book { Title = "Pan", AuthorId = author.Id, Genre = "Roman" }
        );
        await context.SaveChangesAsync();

        var controller = new BookController(context);

        // Act
        var result = await controller.Search("Sult");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Book>>(viewResult.Model);
        Assert.Single(model);
        Assert.Equal("Sult", model.First().Title);
    }

    [Fact]
    public async Task Search_ReturnsMatchingBooks_ByAuthorName()
    {
        // Arrange
        using var context = CreateContext();
        var author1 = new Author { Name = "Knut Hamsun", Nationality = "Norsk" };
        var author2 = new Author { Name = "Sigrid Undset", Nationality = "Norsk" };
        context.Authors.AddRange(author1, author2);
        context.Books.AddRange(
            new Book { Title = "Sult", AuthorId = author1.Id },
            new Book { Title = "Kristin Lavransdatter", AuthorId = author2.Id }
        );
        await context.SaveChangesAsync();

        var controller = new BookController(context);

        // Act
        var result = await controller.Search("Undset");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Book>>(viewResult.Model);
        Assert.Single(model);
        Assert.Equal("Kristin Lavransdatter", model.First().Title);
    }

    [Fact]
    public async Task Search_ReturnsAllBooks_WhenQueryIsEmpty()
    {
        // Arrange
        using var context = CreateContext();
        var author = new Author { Name = "Ibsen", Nationality = "Norsk" };
        context.Authors.Add(author);
        context.Books.AddRange(
            new Book { Title = "Peer Gynt", AuthorId = author.Id },
            new Book { Title = "Et dukkehjem", AuthorId = author.Id }
        );
        await context.SaveChangesAsync();

        var controller = new BookController(context);

        // Act
        var result = await controller.Search("");

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Book>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }
}