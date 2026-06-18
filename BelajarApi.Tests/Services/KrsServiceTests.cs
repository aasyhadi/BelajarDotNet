using BelajarApi.Dtos;
using BelajarApi.Models;
using BelajarApi.Repositories;
using BelajarApi.Services;
using FluentAssertions;
using Moq;
using Xunit;

namespace BelajarApi.Tests.Services;

public class KrsServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_ThrowException_When_KrsAlreadyExists()
    {
        var repositoryMock = new Mock<IKrsRepository>();

        repositoryMock
            .Setup(x => x.ExistsAsync(1, 1))
            .ReturnsAsync(true);

        var service = new KrsService(repositoryMock.Object);

        var dto = new KrsDto
        {
            MahasiswaId = 1,
            MataKuliahId = 1
        };

        var action = async () => await service.CreateAsync(dto);

        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Mata kuliah sudah diambil oleh mahasiswa ini.");
    }

    [Fact]
    public async Task DeleteAsync_Should_ThrowException_When_KrsNotFound()
    {
        var repositoryMock = new Mock<IKrsRepository>();

        repositoryMock
            .Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Krs?)null);

        var service = new KrsService(repositoryMock.Object);

        var action = async () => await service.DeleteAsync(99);

        await action.Should()
            .ThrowAsync<Exception>()
            .WithMessage("Data KRS tidak ditemukan.");
    }

    [Fact]
    public async Task CreateAsync_Should_Return_KrsResponseDto_When_Success()
    {
        // Arrange
        var repositoryMock = new Mock<IKrsRepository>();

        var dto = new KrsDto
        {
            MahasiswaId = 1,
            MataKuliahId = 1
        };

        repositoryMock
            .Setup(x => x.ExistsAsync(1, 1))
            .ReturnsAsync(false);

        repositoryMock
            .Setup(x => x.AddAsync(It.IsAny<Krs>()))
            .Callback<Krs>(krs =>
            {
                krs.Id = 1;
            })
            .Returns(Task.CompletedTask);

        repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(new Krs
            {
                Id = 1,
                MahasiswaId = 1,
                MataKuliahId = 1,
                TanggalAmbil = DateTime.UtcNow,
                Mahasiswa = new Mahasiswa
                {
                    Id = 1,
                    Nama = "Budi"
                },
                MataKuliah = new MataKuliah
                {
                    Id = 1,
                    NamaMataKuliah = "Web API dengan .NET",
                    Sks = 3
                }
            });

        var service = new KrsService(repositoryMock.Object);

        // Act
        var result = await service.CreateAsync(dto);

        // Assert
        result.Should().NotBeNull();
        result.Id.Should().Be(1);
        result.NamaMahasiswa.Should().Be("Budi");
        result.NamaMataKuliah.Should().Be("Web API dengan .NET");
        result.Sks.Should().Be(3);
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_Krs_When_Data_Exists()
    {
        // Arrange
        var repositoryMock = new Mock<IKrsRepository>();

        var existingKrs = new Krs
        {
            Id = 1,
            MahasiswaId = 1,
            MataKuliahId = 1
        };

        repositoryMock
            .Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(existingKrs);

        repositoryMock
            .Setup(x => x.DeleteAsync(existingKrs))
            .Returns(Task.CompletedTask);

        var service = new KrsService(repositoryMock.Object);

        // Act
        await service.DeleteAsync(1);

        // Assert
        repositoryMock.Verify(
            x => x.DeleteAsync(existingKrs),
            Times.Once
        );
    }
}