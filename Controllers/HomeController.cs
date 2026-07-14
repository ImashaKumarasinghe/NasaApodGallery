using Microsoft.AspNetCore.Mvc;
using NasaApodGallery.DTOs;
using NasaApodGallery.Models;
using NasaApodGallery.Repositories.Interfaces;
using NasaApodGallery.Services.Interfaces;

namespace NasaApodGallery.Controllers;

public class HomeController : Controller
{
    private readonly INasaApiService _nasaApiService;
    private readonly IApodRepository _apodRepository;
    private readonly ILogger<HomeController> _logger;

    public HomeController(
        INasaApiService nasaApiService,
        IApodRepository apodRepository,
        ILogger<HomeController> logger)
    {
        _nasaApiService = nasaApiService;
        _apodRepository = apodRepository;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        HomeViewModel viewModel = new()
        {
            StartDate = DateTime.Today.AddDays(-6),
            EndDate = DateTime.Today,
            ApodEntries = await _apodRepository.GetAllAsync()
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Fetch(
        HomeViewModel viewModel)
    {
        if (viewModel.StartDate > viewModel.EndDate)
        {
            viewModel.ErrorMessage =
                "Start date cannot be later than end date.";

            viewModel.ApodEntries =
                await _apodRepository.GetAllAsync();

            return View("Index", viewModel);
        }

        if (viewModel.EndDate.Date > DateTime.Today)
        {
            viewModel.ErrorMessage =
                "End date cannot be in the future.";

            viewModel.ApodEntries =
                await _apodRepository.GetAllAsync();

            return View("Index", viewModel);
        }

        try
        {
            List<ApodDto> nasaEntries =
                await _nasaApiService.GetApodRangeAsync(
                    viewModel.StartDate,
                    viewModel.EndDate);

            int insertedCount =
                await _apodRepository.InsertNewEntriesAsync(
                    nasaEntries);

            viewModel.Message =
                $"{nasaEntries.Count} records received. " +
                $"{insertedCount} new records saved.";

            viewModel.ApodEntries =
                await _apodRepository.GetAllAsync();

            return View("Index", viewModel);
        }
        catch (Exception exception)
        {
            _logger.LogError(
                exception,
                "Error fetching NASA APOD records.");

            viewModel.ErrorMessage =
                "The APOD records could not be fetched. " +
                "Check the API key, internet connection and dates.";

            viewModel.ApodEntries =
                await _apodRepository.GetAllAsync();

            return View("Index", viewModel);
        }
    }
}