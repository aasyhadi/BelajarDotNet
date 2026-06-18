using BelajarApi.Dtos;
using BelajarApi.Models;
using BelajarApi.Repositories;
using BelajarApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace BelajarApi.Tests.Services;

public class AuthServiceTests
{
    private static IConfiguration GetConfiguration()
    {
        return new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                { "JwtSettings:SecretKey", "ini_secret_key_panjang_untuk_testing_123456" },
                { "JwtSettings:Issuer", "BelajarApi" },
                { "JwtSettings:Audience", "BelajarReact" },
                { "JwtSettings:ExpiryMinutes", "60" }
            })
            .Build();
    }

    [Fact]
    public async Task LoginAsync_Should_Return_False_When_Email_Not_Found()
    {
        var userRepo = new Mock<IUserRepository>();

        userRepo
            .Setup(x => x.GetByEmailAsync("notfound@mail.com"))
            .ReturnsAsync((User?)null);

        var service = new AuthService(
            userRepo.Object,
            GetConfiguration()
        );

        var result = await service.LoginAsync(new LoginDto
        {
            Email = "notfound@mail.com",
            Password = "password"
        });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Email atau password salah");
    }

    [Fact]
    public async Task LoginAsync_Should_Return_False_When_Password_Wrong()
    {
        var userRepo = new Mock<IUserRepository>();

        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Email = "admin@mail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password-benar"),
            Role = "Admin"
        };

        userRepo
            .Setup(x => x.GetByEmailAsync("admin@mail.com"))
            .ReturnsAsync(user);

        var service = new AuthService(
            userRepo.Object,
            GetConfiguration()
        );

        var result = await service.LoginAsync(new LoginDto
        {
            Email = "admin@mail.com",
            Password = "password-salah"
        });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Email atau password salah");
    }

    [Fact]
    public async Task LoginAsync_Should_Return_Token_When_Login_Success()
    {
        var userRepo = new Mock<IUserRepository>();

        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Email = "admin@mail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password-benar"),
            Role = "Admin"
        };

        userRepo
            .Setup(x => x.GetByEmailAsync("admin@mail.com"))
            .ReturnsAsync(user);

        userRepo
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        var service = new AuthService(
            userRepo.Object,
            GetConfiguration()
        );

        var result = await service.LoginAsync(new LoginDto
        {
            Email = "admin@mail.com",
            Password = "password-benar"
        });

        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Token.Should().NotBeNullOrWhiteSpace();
        result.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.Data.Email.Should().Be("admin@mail.com");
        result.Data.Role.Should().Be("Admin");
    }

    [Fact]
    public async Task RefreshTokenAsync_Should_Return_False_When_Token_Invalid()
    {
        var userRepo = new Mock<IUserRepository>();

        userRepo
            .Setup(x => x.GetByRefreshTokenAsync("token-salah"))
            .ReturnsAsync((User?)null);

        var service = new AuthService(
            userRepo.Object,
            GetConfiguration()
        );

        var result = await service.RefreshTokenAsync(new RefreshTokenDto
        {
            RefreshToken = "token-salah"
        });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Refresh token tidak valid");
    }

    [Fact]
    public async Task RefreshTokenAsync_Should_Return_False_When_Token_Expired()
    {
        var userRepo = new Mock<IUserRepository>();

        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Email = "admin@mail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = "Admin",
            RefreshToken = "token-lama",
            RefreshTokenExpiryTime = DateTime.Now.AddDays(-1)
        };

        userRepo
            .Setup(x => x.GetByRefreshTokenAsync("token-lama"))
            .ReturnsAsync(user);

        var service = new AuthService(
            userRepo.Object,
            GetConfiguration()
        );

        var result = await service.RefreshTokenAsync(new RefreshTokenDto
        {
            RefreshToken = "token-lama"
        });

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Refresh token sudah expired");
    }

    [Fact]
    public async Task RefreshTokenAsync_Should_Return_New_Token_When_Token_Valid()
    {
        var userRepo = new Mock<IUserRepository>();

        var user = new User
        {
            Id = 1,
            Name = "Admin",
            Email = "admin@mail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = "Admin",
            RefreshToken = "token-lama",
            RefreshTokenExpiryTime = DateTime.Now.AddDays(1)
        };

        userRepo
            .Setup(x => x.GetByRefreshTokenAsync("token-lama"))
            .ReturnsAsync(user);

        userRepo
            .Setup(x => x.UpdateAsync(It.IsAny<User>()))
            .ReturnsAsync(true);

        var service = new AuthService(
            userRepo.Object,
            GetConfiguration()
        );

        var result = await service.RefreshTokenAsync(new RefreshTokenDto
        {
            RefreshToken = "token-lama"
        });

        result.Success.Should().BeTrue();
        result.Data.Should().NotBeNull();
        result.Data!.Token.Should().NotBeNullOrWhiteSpace();
        result.Data.RefreshToken.Should().NotBeNullOrWhiteSpace();
        result.Data.Email.Should().Be("admin@mail.com");
    }
    
}