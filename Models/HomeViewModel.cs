using System.ComponentModel.DataAnnotations;

namespace NasaApodGallery.Models;

public class HomeViewModel
{
    [DataType(DataType.Date)]
    [Display(Name = "Start Date")]
    public DateTime StartDate { get; set; } = DateTime.Today.AddDays(-6);

    [DataType(DataType.Date)]
    [Display(Name = "End Date")]
    public DateTime EndDate { get; set; } = DateTime.Today;

    public List<Apod> ApodEntries { get; set; } = new();

    public string? Message { get; set; }

    public string? ErrorMessage { get; set; }
}