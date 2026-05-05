using System.Text.Json;
using BookApp.Models;

namespace BookApp.Services;

/// <summary>
/// Service for fetching book data from the National Library of Norway API.
/// </summary>
public class BookApiService
{
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Initializes a new instance of BookApiService with an HttpClient.
    /// </summary>
    /// <param name="httpClient">The HTTP client used for API requests.</param>
    public BookApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress = new Uri("https://api.nb.no/catalog/v1/");
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
    }

    /// <summary>
    /// Searches for books from the National Library API by a search term.
    /// </summary>
    /// <param name="query">The search term to query the API with.</param>
    /// <returns>A list of books matching the search term.</returns>
    public async Task<List<ApiBook>> SearchBooksAsync(string query)
    {
        var response = await _httpClient.GetAsync($"items?q={query}&mediatype=books");
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<NbApiResult>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result?._embedded?.Items?
            .Select(i => new ApiBook
            {
                Title = i.Metadata?.Title,
                PublicationYear = i.Metadata?.Date
            })
            .Where(b => b.Title != null)
            .ToList() ?? new List<ApiBook>();
    }
}

/// <summary>
/// Represents the top-level response from the National Library API.
/// </summary>
public class NbApiResult
{
    public NbEmbedded? _embedded { get; set; }
}

/// <summary>
/// Represents the embedded items in the API response.
/// </summary>
public class NbEmbedded
{
    public List<NbItem>? Items { get; set; }
}

/// <summary>
/// Represents a single item returned from the API.
/// </summary>
public class NbItem
{
    public NbMetadata? Metadata { get; set; }
}

/// <summary>
/// Represents metadata for a book item from the API.
/// </summary>
public class NbMetadata
{
    public string? Title { get; set; }
    public string? Date { get; set; }
}

/// <summary>
/// Represents a simplified book object used in the import flow.
/// </summary>
public class ApiBook
{
    public string? Title { get; set; }
    public string? PublicationYear { get; set; }
}