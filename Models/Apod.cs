using System.Text.Json.Serialization;

namespace NasaApodGallery.Models;

public class Apod
{
    public int Id { get; set; }

    public DateTime ApodDate { get; set; }

    public string Title { get; set; } = string.Empty;

    public string? Explanation { get; set; }

    public string Url { get; set; } = string.Empty;

    public string? HdUrl { get; set; }

    public string? ThumbnailUrl { get; set; }

    public string MediaType { get; set; } = string.Empty;

    public string? ServiceVersion { get; set; }

    public string? Copyright { get; set; }

    public DateTime SavedAt { get; set; }
}