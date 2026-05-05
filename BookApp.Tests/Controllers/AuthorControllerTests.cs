using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BookApp.Controllers;
using BookApp.Data;
using BookApp.Models;

namespace BookApp.Tests.Controllers;

/// <summary>
/// Unit tests for AuthorController CRUD operations.
/// </summary>
public class AuthorControllerTests
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
    public async Task Index_ReturnsViewResult_WithListOfAuthors()
    {
        // Arrange
        using var context = CreateContext();
        context.Authors.AddRange(
            new Author { Name = "Sigrid Undset", Nationality = "Norsk" },
            new Author { Name = "Ibsen", Nationality = "Norsk" }
        );
        await context.SaveChangesAsync();

        var controller = new AuthorController(context);

        // Act
        var result = await controller.Index();

        // Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Author>>(viewResult.Model);
        Assert.Equal(2, model.Count());
    }

    [Fact]
    public async Task Create_Post_AddsAuthorToDatabase()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new AuthorController(context);
        var newAuthor = new Author { Name = "Knut Hamsun", Nationality = "Norsk" };

        // Act
        await controller.Create(newAuthor);

        // Assert
        Assert.Equal(1, context.Authors.Count());
        Assert.Equal("Knut Hamsun", context.Authors.First().Name);
    }

    [Fact]
    public async Task Details_ReturnsNotFound_WhenIdIsNull()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new AuthorController(context);

        // Act
        var result = await controller.Details(null);

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
}