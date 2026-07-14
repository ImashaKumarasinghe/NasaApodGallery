using NasaApodGallery.Repositories;
using NasaApodGallery.Repositories.Interfaces;
using NasaApodGallery.Services;
using NasaApodGallery.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddHttpClient<INasaApiService, NasaApiService>();

builder.Services.AddScoped<IApodRepository, ApodRepository>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();