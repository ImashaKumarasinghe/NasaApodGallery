# NASA APOD Gallery

An ASP.NET Core MVC application that retrieves NASA's Astronomy Picture of the Day (APOD) data using the NASA APOD API, stores the data in SQL Server using ADO.NET, and displays the saved records in a responsive gallery.

---

# Technologies Used

- ASP.NET Core MVC (.NET 10)
- C#
- SQL Server / SQL Server Express
- ADO.NET (Microsoft.Data.SqlClient)
- NASA APOD API
- Bootstrap
- HTML5
- CSS3

---

# Features

- Fetch APOD records using a selected date range
- Consume NASA APOD REST API
- Deserialize JSON responses into DTO objects
- Save APOD records to SQL Server using ADO.NET
- Prevent duplicate records based on APOD date
- Read records from SQL Server using SqlDataReader
- Display APOD images in a responsive gallery
- Support both image and video APOD entries (thumbnail for videos)

---

# Project Structure

```
NasaApodGallery
│
├── Controllers
├── Database
│   └── CreateDatabase.sql
├── DTOs
├── Models
├── Repositories
├── Services
├── Views
├── wwwroot
├── appsettings.json
├── Program.cs
├── README.md
└── NasaApodGallery.csproj
```

---

# Requirements

- .NET 10 SDK
- SQL Server or SQL Server Express
- SQL Server Management Studio (SSMS)
- Visual Studio 2022 or Visual Studio Code
- NASA API Key

---

# Database Setup

1. Open SQL Server Management Studio.
2. Connect to your SQL Server instance.
3. Open the file:

```
Database/CreateDatabase.sql
```

4. Execute the script.
5. Verify that the database named **NasaApodDb** is created.
6. Verify that the table **ApodEntries** is created.

---

# SQL Server Connection String

Update **appsettings.json** with your SQL Server connection string.

Example:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=NasaApodDb;Trusted_Connection=True;TrustServerCertificate=True;"
  },

  "NasaApi": {
    "BaseUrl": "https://api.nasa.gov/planetary/apod",
    "ApiKey": "ADD_YOUR_NASA_API_KEY_HERE"
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },

  "AllowedHosts": "*"
}
```

---

# NASA API Key Setup

1. Visit:

https://api.nasa.gov/

2. Generate a free API key.

3. Create a file named:

```
appsettings.Development.json
```

4. Add your API key:

```json
{
  "NasaApi": {
    "ApiKey": "YOUR_REAL_NASA_API_KEY"
  }
}
```

> **Important**
>
> Never commit your real API key to GitHub.

---

# .gitignore

Create a file named **.gitignore** in the project root and add:

```
bin/
obj/
.vs/
appsettings.Development.json
secrets.json
```

This prevents temporary files and your API key from being uploaded to GitHub.

---

# Restore Packages

Open a terminal inside the project folder and run:

```bash
dotnet restore
```

---

# Build the Project

```bash
dotnet build
```

---

# Run the Project

```bash
dotnet run
```

The terminal will display a localhost URL similar to:

```
http://localhost:5234
```

Open the URL in your web browser.

---

# Application Flow

1. User selects a Start Date.
2. User selects an End Date.
3. User clicks **Fetch and Save**.
4. The application calls the NASA APOD API.
5. NASA returns APOD data as JSON.
6. JSON is deserialized into DTO objects.
7. New records are inserted into SQL Server.
8. Duplicate records are ignored.
9. Records are read from SQL Server.
10. Images and titles are displayed in a responsive gallery.

---

# Functional Tests

## Test 1 – Fetch APOD Records

- Select a small date range.
- Click **Fetch and Save**.

Expected Result:

- Records are received.
- Records are saved.
- Gallery is displayed.

---

## Test 2 – Duplicate Prevention

Select the same date range again.

Expected Result:

```
0 new records saved
```

---

## Test 3 – Verify SQL Server

Run the following query in SQL Server Management Studio:

```sql
USE NasaApodDb;
GO

SELECT *
FROM ApodEntries
ORDER BY ApodDate DESC;
```

Expected Result:

Previously fetched APOD records should appear.

---

## Test 4 – Invalid Date Range

Select a Start Date later than the End Date.

Expected Result:

```
Start date cannot be later than end date.
```

---

## Test 5 – Video APOD

When the NASA API returns:

```
media_type = video
```

The application displays the returned thumbnail instead of trying to display the video URL as an image.

---

# GitHub Upload

Initialize Git:

```bash
git init
```

Add files:

```bash
git add .
```

Commit:

```bash
git commit -m "Initial NASA APOD Gallery"
```

Rename branch:

```bash
git branch -M main
```

Connect your repository:

```bash
git remote add origin https://github.com/YOUR_USERNAME/NasaApodGallery.git
```

Push:

```bash
git push -u origin main
```

---

# Files Included

- Controllers
- DTOs
- Models
- Services
- Repositories
- Views
- wwwroot
- Database/CreateDatabase.sql
- README.md
- appsettings.json
- Program.cs
- NasaApodGallery.csproj

---

# Files Not Included

The following files should **NOT** be uploaded to GitHub:

- bin/
- obj/
- .vs/
- appsettings.Development.json
- secrets.json
- Real NASA API Key
- Database passwords

---

# Author


Imasha Kumarasinghe