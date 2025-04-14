using System;

namespace WebApplication1.Services;

public class ImageServices
{

    private readonly IWebHostEnvironment _env;
    private const string Image_Folder = "uploads/projects/images/";

    public ImageServices(IWebHostEnvironment env)
    {
        _env = env;
    }

    public async Task<string> SaveImage(IFormFile imageFile)
    {
        if (string.IsNullOrEmpty(_env.WebRootPath))
        {
            _env.WebRootPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        }
        var uploadsFolder = Path.Combine(_env.WebRootPath, Image_Folder);

        if (!Directory.Exists(uploadsFolder))
        {
            Directory.CreateDirectory(uploadsFolder);
        }

        var uniqueFileName = $"{Guid.NewGuid()}_{imageFile.FileName}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(fileStream);
        }


        string imageUrl = $"/{Image_Folder}/{uniqueFileName}";

        return imageUrl;

    }

    public void DeleteImage(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return;
        }

        var fullPath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        else
        {
            throw new FileNotFoundException($"File not found: {fullPath}");
        }
    }

    public string GetImagePath(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return string.Empty;
        }

        var fullPath = Path.Combine(_env.WebRootPath, imagePath.TrimStart('/'));
        if (File.Exists(fullPath))
        {
            return fullPath;
        }
        else
        {
            throw new FileNotFoundException($"File not found: {fullPath}");
        }
    }
    public static IFormFile? ConvertImagePathToFile(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
        {
            return null;
        }

        var fileStream = new FileStream(imagePath, FileMode.Open, FileAccess.Read);
        return new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(imagePath))
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream"
        };
    }
}