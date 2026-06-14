using BelajarApi.Dtos;
using FluentValidation;

namespace BelajarApi.Validators;

public class MahasiswaDtoValidator : AbstractValidator<MahasiswaDto>
{
    public MahasiswaDtoValidator()
    {
        RuleFor(x => x.Nama)
            .NotEmpty()
            .WithMessage("Nama wajib diisi")
            .MaximumLength(100)
            .WithMessage("Nama maksimal 100 karakter");

        RuleFor(x => x.Jurusan)
            .NotEmpty()
            .WithMessage("Jurusan wajib diisi")
            .MaximumLength(100)
            .WithMessage("Jurusan maksimal 100 karakter");
    }
}