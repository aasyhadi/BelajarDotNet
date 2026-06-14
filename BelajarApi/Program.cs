using BelajarApi.Data;
using BelajarApi.Endpoints;
using Microsoft.EntityFrameworkCore;
using BelajarApi.Repositories;
using BelajarApi.Services;
using BelajarApi.Validators;
using FluentValidation;
using BelajarApi.Dtos;
using BelajarApi.Middlewares;
using BelajarApi.Mappings;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"]!;

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

builder.Services.AddScoped<IMahasiswaRepository, MahasiswaRepository>();
builder.Services.AddScoped<IMahasiswaService, MahasiswaService>();
builder.Services.AddScoped<IMataKuliahRepository, MataKuliahRepository>();
builder.Services.AddScoped<IMataKuliahService, MataKuliahService>();
builder.Services.AddScoped<IValidator<MahasiswaDto>, MahasiswaDtoValidator>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(secretKey))
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

builder.Services.AddScoped<
    IAuditLogRepository,
    AuditLogRepository>();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();

var app = builder.Build();

app.UseMiddleware<GlobalExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapMahasiswaEndpoints();
app.MapMataKuliahEndpoints();
app.MapAuthEndpoints();

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

app.MapAuditLogEndpoints();

app.Run();