using NasaApodGallery.DTOs;
using NasaApodGallery.Models;

namespace NasaApodGallery.Repositories.Interfaces;

public interface IApodRepository
{
    Task<int> InsertNewEntriesAsync(IEnumerable<ApodDto> entries);

    Task<List<Apod>> GetAllAsync();
}