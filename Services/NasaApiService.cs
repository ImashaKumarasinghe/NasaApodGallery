using System.Net.Http.Json;
using NasaApodGallery.DTOs;
using NasaApodGallery.Services.Interfaces;

namespace NasaApodGallery.Services;

public class NasaApiService : INasaApiService
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;
    private readonly ILogger<NasaApiService> _logger;

    public NasaApiService(
        HttpClient httpClient,
        IConfiguration configuration,
        ILogger<NasaApiService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<List<ApodDto>> GetApodRangeAsync(
        DateTime startDate,
        DateTime endDate)
    {
        string? baseUrl = _configuration["NasaApi:BaseUrl"];
        string? apiKey = _configuration["NasaApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(baseUrl))
        {
            throw new InvalidOperationException(
                "NASA API base URL is missing.");
        }

        if (string.IsNullOrWhiteSpace(apiKey) ||
            apiKey == "ADD_YOUR_NASA_API_KEY_HERE")
        {
            throw new InvalidOperationException(
                "NASA API key is missing.");
        }

        string requestUrl =
            $"{baseUrl}" +
            $"?api_key={Uri.EscapeDataString(apiKey)}" +
            $"&start_date={startDate:yyyy-MM-dd}" +
            $"&end_date={endDate:yyyy-MM-dd}" +
            $"&thumbs=true";

        _logger.LogInformation(
            "Requesting NASA APOD records from {StartDate} to {EndDate}",
            startDate,
            endDate);

        using HttpResponseMessage response =
            await _httpClient.GetAsync(requestUrl);

        if (!response.IsSuccessStatusCode)
        {
            string errorContent =
                await response.Content.ReadAsStringAsync();

            throw new HttpRequestException(
                $"NASA API request failed. " +
                $"Status: {(int)response.StatusCode}. " +
                $"Response: {errorContent}");
        }

        List<ApodDto>? records =
            await response.Content.ReadFromJsonAsync<List<ApodDto>>();

        return records ?? new List<ApodDto>();
    }
}