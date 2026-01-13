using System;
using System.IO;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using HCAMiniEHR.Data;

var settingsPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

// Validate appsettings.json before building the host to avoid Json parse exceptions thrown by CreateBuilder
if (File.Exists(settingsPath))
{
    try
    {
        var text = File.ReadAllText(settingsPath);
        if (string.IsNullOrWhiteSpace(text))
        {
            throw new JsonException("appsettings.json is empty.");
        }

        using var _ = JsonDocument.Parse(text); // will throw on invalid JSON
    }
    catch (Exception ex) when (ex is JsonException || ex is FormatException || ex is System.Text.Json.JsonException)
    {
        var backupPath = settingsPath + "." + DateTime.UtcNow.ToString("yyyyMMddHHmmss") + ".bak";
        File.Move(settingsPath, backupPath);
        File.WriteAllText(settingsPath, "{}"); // minimal valid JSON so CreateBuilder can continue
        Console.WriteLine($"Warning: invalid JSON in 'appsettings.json'. Backed up to '{backupPath}' and created a minimal 'appsettings.json'. Please fix the original file.");
    }
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<MINIDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
