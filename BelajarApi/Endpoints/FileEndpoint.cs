namespace BelajarApi.Endpoints;

public static class FileEndpoint
{
    private static readonly string[] AllowedExtensions =
    [
        ".pdf",
        ".jpg",
        ".jpeg",
        ".png"
    ];

    private const long MaxFileSize = 5 * 1024 * 1024;

    private static bool IsValidFile(IFormFile file, out string message)
    {
        var extension = Path
            .GetExtension(file.FileName)
            .ToLowerInvariant();

        if (!AllowedExtensions.Contains(extension))
        {
            message = "Format file tidak didukung. Hanya PDF, JPG, JPEG, dan PNG.";
            return false;
        }

        if (file.Length > MaxFileSize)
        {
            message = "Ukuran file maksimal 5 MB.";
            return false;
        }

        message = "";
        return true;
    }

    public static void MapFileEndpoints(this WebApplication app)
    {
        app.MapPost("/upload", async (HttpRequest request) =>
        {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest(new
                {
                    message = "Request harus multipart/form-data."
                });
            }

            var form = await request.ReadFormAsync();
            var file = form.Files["file"];

            if (file == null || file.Length == 0)
            {
                return Results.BadRequest(new
                {
                    message = "File kosong atau tidak ditemukan."
                });
            }

            if (!IsValidFile(file, out var validationMessage))
            {
                return Results.BadRequest(new
                {
                    success = false,
                    message = validationMessage
                });
            }

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            Directory.CreateDirectory(uploadsFolder);

            var originalFileName = Path.GetFileName(file.FileName);
            var fileName = $"{Guid.NewGuid()}_{originalFileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            await using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Results.Ok(new
            {
                success = true,
                message = "File berhasil diupload",
                data = new
                {
                    fileName,
                    url = $"/uploads/{fileName}"
                }
            });
        })
        .RequireAuthorization();

        app.MapPost("/upload-multiple", async (HttpRequest request) =>
        {
            if (!request.HasFormContentType)
            {
                return Results.BadRequest(new
                {
                    message = "Request harus multipart/form-data."
                });
            }

            var form = await request.ReadFormAsync();
            var files = form.Files;

            if (files == null || files.Count == 0)
            {
                return Results.BadRequest(new
                {
                    message = "File kosong atau tidak ditemukan."
                });
            }

            var uploadsFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "wwwroot",
                "uploads"
            );

            Directory.CreateDirectory(uploadsFolder);

            var uploadedFiles = new List<object>();

            foreach (var file in files)
            {
                if (file.Length == 0)
                {
                    continue;
                }

                if (!IsValidFile(file, out var validationMessage))
                {
                    return Results.BadRequest(new
                    {
                        success = false,
                        message = $"{file.FileName}: {validationMessage}"
                    });
                }

                var originalFileName = Path.GetFileName(file.FileName);
                var fileName = $"{Guid.NewGuid()}_{originalFileName}";
                var filePath = Path.Combine(uploadsFolder, fileName);

                await using var stream = new FileStream(filePath, FileMode.Create);
                await file.CopyToAsync(stream);

                uploadedFiles.Add(new
                {
                    fileName,
                    url = $"/uploads/{fileName}"
                });
            }

            return Results.Ok(new
            {
                success = true,
                message = "File berhasil diupload",
                data = uploadedFiles
            });
        })
        .RequireAuthorization();
    }
}