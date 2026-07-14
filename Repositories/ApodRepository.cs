using System.Data;
using Microsoft.Data.SqlClient;
using NasaApodGallery.DTOs;
using NasaApodGallery.Models;
using NasaApodGallery.Repositories.Interfaces;

namespace NasaApodGallery.Repositories;

public class ApodRepository : IApodRepository
{
    private readonly string _connectionString;
    private readonly ILogger<ApodRepository> _logger;

    public ApodRepository(
        IConfiguration configuration,
        ILogger<ApodRepository> logger)
    {
        _connectionString =
            configuration.GetConnectionString("DefaultConnection")
            ?? throw new InvalidOperationException(
                "Database connection string is missing.");

        _logger = logger;
    }

    public async Task<int> InsertNewEntriesAsync(
        IEnumerable<ApodDto> entries)
    {
        int insertedCount = 0;

        const string sql = """
            IF NOT EXISTS
            (
                SELECT 1
                FROM ApodEntries
                WHERE ApodDate = @ApodDate
            )
            BEGIN
                INSERT INTO ApodEntries
                (
                    ApodDate,
                    Title,
                    Explanation,
                    Url,
                    HdUrl,
                    ThumbnailUrl,
                    MediaType,
                    ServiceVersion,
                    Copyright,
                    SavedAt
                )
                VALUES
                (
                    @ApodDate,
                    @Title,
                    @Explanation,
                    @Url,
                    @HdUrl,
                    @ThumbnailUrl,
                    @MediaType,
                    @ServiceVersion,
                    @Copyright,
                    SYSUTCDATETIME()
                );

                SELECT 1;
            END
            ELSE
            BEGIN
                SELECT 0;
            END
            """;

        await using SqlConnection connection =
            new(_connectionString);

        await connection.OpenAsync();

        foreach (ApodDto entry in entries)
        {
            if (!DateTime.TryParse(entry.Date, out DateTime apodDate))
            {
                _logger.LogWarning(
                    "Invalid NASA APOD date: {Date}",
                    entry.Date);

                continue;
            }

            await using SqlCommand command =
                new(sql, connection);

            command.Parameters.Add(
                "@ApodDate",
                SqlDbType.Date).Value = apodDate.Date;

            command.Parameters.Add(
                "@Title",
                SqlDbType.NVarChar,
                300).Value = entry.Title;

            command.Parameters.Add(
                "@Explanation",
                SqlDbType.NVarChar,
                -1).Value =
                    (object?)entry.Explanation ?? DBNull.Value;

            command.Parameters.Add(
                "@Url",
                SqlDbType.NVarChar,
                2048).Value = entry.Url;

            command.Parameters.Add(
                "@HdUrl",
                SqlDbType.NVarChar,
                2048).Value =
                    (object?)entry.HdUrl ?? DBNull.Value;

            command.Parameters.Add(
                "@ThumbnailUrl",
                SqlDbType.NVarChar,
                2048).Value =
                    (object?)entry.ThumbnailUrl ?? DBNull.Value;

            command.Parameters.Add(
                "@MediaType",
                SqlDbType.NVarChar,
                50).Value = entry.MediaType;

            command.Parameters.Add(
                "@ServiceVersion",
                SqlDbType.NVarChar,
                20).Value =
                    (object?)entry.ServiceVersion ?? DBNull.Value;

            command.Parameters.Add(
                "@Copyright",
                SqlDbType.NVarChar,
                300).Value =
                    (object?)entry.Copyright ?? DBNull.Value;

            object? result = await command.ExecuteScalarAsync();

            insertedCount += Convert.ToInt32(result);
        }

        return insertedCount;
    }

    public async Task<List<Apod>> GetAllAsync()
    {
        List<Apod> records = new();

        const string sql = """
            SELECT
                Id,
                ApodDate,
                Title,
                Explanation,
                Url,
                HdUrl,
                ThumbnailUrl,
                MediaType,
                ServiceVersion,
                Copyright,
                SavedAt
            FROM ApodEntries
            ORDER BY ApodDate DESC;
            """;

        await using SqlConnection connection =
            new(_connectionString);

        await using SqlCommand command =
            new(sql, connection);

        await connection.OpenAsync();

        await using SqlDataReader reader =
            await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            Apod record = new()
            {
                Id = reader.GetInt32(
                    reader.GetOrdinal("Id")),

                ApodDate = reader.GetDateTime(
                    reader.GetOrdinal("ApodDate")),

                Title = reader.GetString(
                    reader.GetOrdinal("Title")),

                Explanation = GetNullableString(
                    reader,
                    "Explanation"),

                Url = reader.GetString(
                    reader.GetOrdinal("Url")),

                HdUrl = GetNullableString(
                    reader,
                    "HdUrl"),

                ThumbnailUrl = GetNullableString(
                    reader,
                    "ThumbnailUrl"),

                MediaType = reader.GetString(
                    reader.GetOrdinal("MediaType")),

                ServiceVersion = GetNullableString(
                    reader,
                    "ServiceVersion"),

                Copyright = GetNullableString(
                    reader,
                    "Copyright"),

                SavedAt = reader.GetDateTime(
                    reader.GetOrdinal("SavedAt"))
            };

            records.Add(record);
        }

        return records;
    }

    private static string? GetNullableString(
        SqlDataReader reader,
        string columnName)
    {
        int ordinal = reader.GetOrdinal(columnName);

        return reader.IsDBNull(ordinal)
            ? null
            : reader.GetString(ordinal);
    }
}