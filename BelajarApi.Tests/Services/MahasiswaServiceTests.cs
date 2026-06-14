using AutoMapper;
using BelajarApi.Dtos;
using BelajarApi.Repositories;
using BelajarApi.Services;
using FluentAssertions;
using Moq;
using Xunit;
using BelajarApi.Models;

namespace BelajarApi.Tests.Services;

public class MahasiswaServiceTests
{
    [Fact]
    public async Task CreateAsync_Should_Return_False_When_Nama_Is_Empty()
    {
        var mahasiswaRepo = new Mock<IMahasiswaRepository>();
        var mapper = new Mock<IMapper>();
        var auditRepo = new Mock<IAuditLogRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        var service = new MahasiswaService(
            mahasiswaRepo.Object,
            mapper.Object,
            auditRepo.Object,
            currentUser.Object
        );

        var input = new MahasiswaDto
        {
            Nama = "",
            Jurusan = "Teknik Informatika"
        };

        var result = await service.CreateAsync(input);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Nama wajib diisi");
    }

    [Fact]
    public async Task CreateAsync_Should_Return_True_When_Input_Valid()
    {
        var mahasiswaRepo = new Mock<IMahasiswaRepository>();
        var mapper = new Mock<IMapper>();
        var auditRepo = new Mock<IAuditLogRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        var input = new MahasiswaDto
        {
            Nama = "Budi",
            Jurusan = "Teknik Informatika"
        };

        var mahasiswa = new Mahasiswa
        {
            Id = 1,
            Nama = "Budi",
            Jurusan = "Teknik Informatika"
        };

        mapper.Setup(x => x.Map<Mahasiswa>(input))
            .Returns(mahasiswa);

        mahasiswaRepo.Setup(x => x.CreateAsync(mahasiswa))
            .ReturnsAsync(mahasiswa);

        mapper.Setup(x => x.Map<MahasiswaResponseDto>(mahasiswa))
            .Returns(new MahasiswaResponseDto
            {
                Id = 1,
                Nama = "Budi",
                Jurusan = "Teknik Informatika"
            });

        var service = new MahasiswaService(
            mahasiswaRepo.Object,
            mapper.Object,
            auditRepo.Object,
            currentUser.Object
        );

        var result = await service.CreateAsync(input);

        result.Success.Should().BeTrue();
        result.Message.Should().Be("Data mahasiswa berhasil ditambahkan");
        result.Data!.Nama.Should().Be("Budi");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_Data_When_Mahasiswa_Exists()
    {
        var mahasiswaRepo = new Mock<IMahasiswaRepository>();
        var mapper = new Mock<IMapper>();
        var auditRepo = new Mock<IAuditLogRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        var mahasiswa = new Mahasiswa
        {
            Id = 1,
            Nama = "Budi",
            Jurusan = "Teknik Informatika"
        };

        mahasiswaRepo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(mahasiswa);

        mapper.Setup(x => x.Map<MahasiswaResponseDto>(mahasiswa))
            .Returns(new MahasiswaResponseDto
            {
                Id = 1,
                Nama = "Budi",
                Jurusan = "Teknik Informatika"
            });

        var service = new MahasiswaService(
            mahasiswaRepo.Object,
            mapper.Object,
            auditRepo.Object,
            currentUser.Object
        );

        var result = await service.GetByIdAsync(1);

        result.Success.Should().BeTrue();
        result.Data!.Id.Should().Be(1);
        result.Data.Nama.Should().Be("Budi");
    }

    [Fact]
    public async Task GetByIdAsync_Should_Return_False_When_Mahasiswa_Not_Found()
    {
        var mahasiswaRepo = new Mock<IMahasiswaRepository>();
        var mapper = new Mock<IMapper>();
        var auditRepo = new Mock<IAuditLogRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        mahasiswaRepo.Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Mahasiswa?)null);

        var service = new MahasiswaService(
            mahasiswaRepo.Object,
            mapper.Object,
            auditRepo.Object,
            currentUser.Object
        );

        var result = await service.GetByIdAsync(99);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Data mahasiswa tidak ditemukan");
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_True_When_Input_Valid()
    {
        var mahasiswaRepo = new Mock<IMahasiswaRepository>();
        var mapper = new Mock<IMapper>();
        var auditRepo = new Mock<IAuditLogRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        var mahasiswa = new Mahasiswa
        {
            Id = 1,
            Nama = "Budi Lama",
            Jurusan = "Teknik Informatika"
        };

        var input = new MahasiswaDto
        {
            Nama = "Budi Baru",
            Jurusan = "Sistem Informasi"
        };

        mahasiswaRepo.Setup(x => x.GetByIdAsync(1))
            .ReturnsAsync(mahasiswa);

        mahasiswaRepo.Setup(x => x.UpdateAsync(mahasiswa))
            .ReturnsAsync(true);

        mapper.Setup(x => x.Map(input, mahasiswa))
            .Callback(() =>
            {
                mahasiswa.Nama = input.Nama;
                mahasiswa.Jurusan = input.Jurusan;
            });

        mapper.Setup(x => x.Map<MahasiswaResponseDto>(mahasiswa))
            .Returns(new MahasiswaResponseDto
            {
                Id = 1,
                Nama = "Budi Baru",
                Jurusan = "Sistem Informasi"
            });

        var service = new MahasiswaService(
            mahasiswaRepo.Object,
            mapper.Object,
            auditRepo.Object,
            currentUser.Object
        );

        var result = await service.UpdateAsync(1, input);

        result.Success.Should().BeTrue();
        result.Data!.Nama.Should().Be("Budi Baru");
        result.Data.Jurusan.Should().Be("Sistem Informasi");
    }

    [Fact]
    public async Task UpdateAsync_Should_Return_False_When_Mahasiswa_Not_Found()
    {
        var mahasiswaRepo = new Mock<IMahasiswaRepository>();
        var mapper = new Mock<IMapper>();
        var auditRepo = new Mock<IAuditLogRepository>();
        var currentUser = new Mock<ICurrentUserService>();

        var input = new MahasiswaDto
        {
            Nama = "Budi",
            Jurusan = "Teknik Informatika"
        };

        mahasiswaRepo.Setup(x => x.GetByIdAsync(99))
            .ReturnsAsync((Mahasiswa?)null);

        var service = new MahasiswaService(
            mahasiswaRepo.Object,
            mapper.Object,
            auditRepo.Object,
            currentUser.Object
        );

        var result = await service.UpdateAsync(99, input);

        result.Success.Should().BeFalse();
        result.Message.Should().Be("Data mahasiswa tidak ditemukan");
    }
}