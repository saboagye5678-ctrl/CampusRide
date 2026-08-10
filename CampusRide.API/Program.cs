using CampusRide.API.Settings;
using CampusRide.API.Data;
using CampusRide.API.Services;
using CampusRide.API.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;

var builder = WebApplication.CreateBuilder(args);

// ============================================
// CONFIGURE MONGODB FROM ENVIRONMENT VARIABLES
// ============================================
var mongoConnectionString = Environment.GetEnvironmentVariable("MONGODB_CONNECTION_STRING");
var mongoDatabaseName = Environment.GetEnvironmentVariable("MONGODB_DATABASE_NAME") ?? "CampusRide";

if (string.IsNullOrEmpty(mongoConnectionString))
{
    // Fallback to appsettings.json for local development
    mongoConnectionString = builder.Configuration.GetConnectionString("MongoDB") 
        ?? builder.Configuration.GetValue<string>("MongoDB:ConnectionString")
        ?? "mongodb://localhost:27017";
    Console.WriteLine("⚠️ Using local MongoDB connection");
}
else
{
    Console.WriteLine("✅ Using MongoDB Atlas connection from environment variable");
}

// Configure MongoDB settings
builder.Services.Configure<MongoDBSettings>(options =>
{
    options.ConnectionString = mongoConnectionString;
    options.DatabaseName = mongoDatabaseName;
});

// ============================================
// REGISTER SERVICES
// ============================================
builder.Services.AddSingleton<MongoDBService>();

// Repositories
builder.Services.AddSingleton<AuthRepository>();
builder.Services.AddSingleton<LocationRepository>();
builder.Services.AddSingleton<DriverRepository>();
builder.Services.AddSingleton<RideRepository>();
builder.Services.AddSingleton<NoticeRepository>();

builder.Services.AddControllers();

// ============================================
// SWAGGER
// ============================================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ============================================
// CORS - Allow Frontend Domains
// ============================================
var frontendUrls = new[]
{
    "http://localhost:5500",
    "http://localhost:3000",
    "http://127.0.0.1:5500",
    "https://localhost:5500",
    Environment.GetEnvironmentVariable("FRONTEND_URL") ?? "",
    "https://campusride-frontend.onrender.com",
    "https://campusride.onrender.com"
}.Where(url => !string.IsNullOrEmpty(url)).ToArray();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CampusRidePolicy", policy =>
    {
        policy.WithOrigins(frontendUrls)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    });
});

Console.WriteLine($"✅ CORS allowed origins: {string.Join(", ", frontendUrls)}");

// ============================================
// CONFIGURE PORT FOR RENDER
// ============================================
var port = Environment.GetEnvironmentVariable("PORT") ?? "10000";
builder.WebHost.UseUrls($"http://0.0.0.0:{port}");

Console.WriteLine($"🚀 Server running on port {port}");

// ============================================
// BUILD APP
// ============================================
var app = builder.Build();

// ============================================
// DEVELOPMENT VS PRODUCTION
// ============================================
var isProduction = !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("RENDER"));

if (isProduction)
{
    Console.WriteLine("🌐 Running in PRODUCTION mode");
}
else
{
    Console.WriteLine("💻 Running in DEVELOPMENT mode");
}

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment() || !isProduction)
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    // In production, maybe use a simpler setup or keep Swagger for testing
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CampusRidePolicy");
app.UseAuthorization();
app.MapControllers();

// ============================================
// HEALTH CHECK ENDPOINT
// ============================================
app.MapGet("/health", () => new 
{ 
    status = "OK", 
    timestamp = DateTime.UtcNow,
    environment = isProduction ? "Production" : "Development",
    mongodb = string.IsNullOrEmpty(mongoConnectionString) ? "Not configured" : "Configured"
});

Console.WriteLine("✅ Application started successfully!");
Console.WriteLine($"📊 Health check: http://0.0.0.0:{port}/health");

app.Run();
