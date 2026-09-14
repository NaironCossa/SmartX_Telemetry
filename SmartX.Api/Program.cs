using Microsoft.AspNetCore.Mvc;
using SmartX_Telemetry.Models;
using SmartX_Telemetry.Services;
using SmartX_Telemetry.Exceptions;
using System.Security.Cryptography;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Endpoint 1: High-Performance Sensor Data Ingestion WITH SECURITY VALIDATION
app.MapPost("/api/telemetry", ([FromBody] TelemetryPacket<double> packet) =>
{
    try
    {
        // Enforce our custom security checks
        TelemetryValidator.ValidatePacket(packet);

        Console.WriteLine($"Received SECURE telemetry from {packet.DeviceId} at {packet.Timestamp}");
        return Results.Ok(new { Status = "Success", Message = "Telemetry ingested optimally and securely." });
    }
    catch (TelemetryValidationException ex)
    {
        Console.WriteLine($"REJECTED: {ex.Message}");
        return Results.BadRequest(new { Status = "Security Alert", Message = ex.Message });
    }
    catch (Exception ex)
    {
        return Results.Problem("An unexpected server error occurred during ingestion.");
    }
});

// Endpoint 2: Encrypted Log/Media Attachment (Unchanged)
app.MapPost("/api/upload-log", async (IFormFile file) =>
{
    if (file == null || file.Length == 0)
        return Results.BadRequest("Invalid multipart file stream.");

    var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "SecureStorage");
    Directory.CreateDirectory(uploadsPath);

    var filePath = Path.Combine(uploadsPath, $"{Guid.NewGuid()}_{file.FileName}.enc");

    using (Aes aes = Aes.Create())
    {
        aes.Key = new byte[32];
        aes.IV = new byte[16];

        using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
        using (CryptoStream cryptoStream = new CryptoStream(fileStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
        {
            await file.CopyToAsync(cryptoStream);
        }
    }

    return Results.Ok(new { Status = "Encrypted", File = file.FileName, Path = filePath });
});

app.Run();