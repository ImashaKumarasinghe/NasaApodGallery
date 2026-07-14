using NasaApodGallery.DTOs;

namespace NasaApodGallery.Services.Interfaces;

public interface INasaApiService
{
    Task<List<ApodDto>> GetApodRangeAsync(
        DateTime startDate,
        DateTime endDate);
}