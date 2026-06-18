using System.Text;
using BelajarApi.Data;
using BelajarApi.Dtos;
using BelajarApi.Endpoints;
using BelajarApi.Mappings;
using BelajarApi.Middlewares;
using BelajarApi.Models;
using BelajarApi.Repositories;
using BelajarApi.Services;
using BelajarApi.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

// =========================
// Database
// =========================
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// =========================
// Repositories
// =========================
builder.Services.AddScoped<IMahasiswaRepository, MahasiswaRepository>();
builder.Services.AddScoped<IMataKuliahRepository, MataKuliahRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuditLogRepository, AuditLogRepository>();
builder.Services.AddScoped<IKrsRepository, KrsRepository>();

// =========================
// Services
// =========================
builder.Services.AddScoped<IMahasiswaService, MahasiswaService>();
builder.Services.AddScoped<IMataKuliahService, MataKuliahService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IKrsService, KrsService>();

builder.Services.AddHttpContextAccessor();

// =========================
// Validation & Mapping
// =========================
builder.Services.AddScoped<IValidator<MahasiswaDto>, MahasiswaDtoValidator>();
builder.Services.AddAutoMapper(typeof(MappingProfile));

// =========================
// Swagger
// =========================
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// =========================
// Static File
// =========================
builder.Services.AddDirectoryBrowser();

// =========================
// CORS
// =========================
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// =========================
// Authentication
// =========================
builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme =
            JwtBearerDefaults.AuthenticationScheme;

        options.DefaultChallengeScheme =
            JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters =
            new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,

                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(
                    Encoding.UTF8.GetBytes(secretKey)
                )
            };
    });

// =========================
// Authorization
// =========================
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin", "Super Admin"));

    options.AddPolicy("SuperAdminOnly", policy =>
        policy.RequireRole("Super Admin"));
});

var app = builder.Build();

// =========================
// Middleware
// =========================
app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowReactApp");

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

// =========================
// Endpoints
// =========================
app.MapAuthEndpoints();
app.MapMahasiswaEndpoints();
app.MapMataKuliahEndpoints();
app.MapKrsEndpoints();
app.MapUserEndpoints();
app.MapAuditLogEndpoints();
app.MapDashboardEndpoints();
app.MapFileEndpoints();

app.MapGet("/hello", () =>
{
    return Results.Ok(new
    {
        message = "Halo Mahasiswa .NET",
        status = "API berhasil dibuat",
        hari = "Hari 1"
    });
});

app.MapGet("/error-test", () =>
{
    throw new Exception("Ini error test");
});

app.Run();