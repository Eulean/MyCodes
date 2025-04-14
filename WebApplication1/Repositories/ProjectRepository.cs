using System;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Models;
using WebApplication1.Models.Dtos;
using WebApplication1.Services;

namespace WebApplication1.Repositories;

public class ProjectRepository
{
    private readonly MyContext _context;
    private readonly ImageServices _imageServices;

    public ProjectRepository(MyContext context, ImageServices imageServices)
    {
        _context = context;
        _imageServices = imageServices;
    }

    public async Task<ProjectDto> GetById(int id, CancellationToken cancellationToken = default)
    {
        var project = await _context.Projects.FirstOrDefaultAsync(p => p.Id == id && !p.IsDeleted, cancellationToken);


        if (project == null)
        {
            throw new KeyNotFoundException($"Project with Id : {id} not found.");
        }

        ProjectDto response = MappingProjectDto(new ProjectDto(), project);

        return response;
    }

    public async Task<ProjectDto> CreateProject(ProjectDto projectDto)
    {
        if (projectDto == null)
        {
            throw new ArgumentNullException(nameof(projectDto));
        }

        var project = new Projects
        {
            Name = projectDto.Name,
            Description = projectDto.Description,
            // ImageAlt = projectDto.ImageAlt,
            Link = projectDto.Link ?? string.Empty,
            RepositoryUrl = projectDto.RepositoryUrl,
            Category = projectDto.Category ?? string.Empty,
            Technology = projectDto.Technology ?? string.Empty,
            Status = projectDto.Status ?? string.Empty,
            TeamMembers = projectDto.TeamMembers,
            Challenges = projectDto.Challenges,
            Outcomes = projectDto.Outcomes,
            Summary = projectDto.Summary,
            IsFeatured = true,
            CreatedAt = DateTime.UtcNow,
            // UpdatedAt = DateTime.UtcNow,
            // DeletedAt = null,
            IsDeleted = false,


        };

        if (projectDto.ImageFile != null)
        {
            project.Image = await _imageServices.SaveImage(projectDto.ImageFile);
            project.ImageAlt = project.Image;
        }

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        ProjectDto response = MappingProjectDto(projectDto, project);

        return response;
    }

    public async Task<Projects> UpdateProject(int id, ProjectDto projectDto)
    {
        if (projectDto == null)
        {
            throw new ArgumentNullException(nameof(projectDto));
        }

        var project = await _context.Projects.FindAsync(id);

        if (project == null)
        {
            throw new KeyNotFoundException($"Project with ID {id} not found.");
        }

        if (projectDto.ImageFile != null)
        {
            _imageServices.DeleteImage(project.Image);

            project.Image = await _imageServices.SaveImage(projectDto.ImageFile);
            project.ImageAlt = project.Image;
        }

        UpdateMapProject(projectDto, project);

        await _context.SaveChangesAsync();
        return project;

    }

    private static void UpdateMapProject(ProjectDto projectDto, Projects project)
    {
        project.Name = projectDto.Name;
        project.Description = projectDto.Description;
        // project.ImageAlt = projectDto.ImageAlt;
        project.Link = projectDto.Link ?? project.Link;
        project.RepositoryUrl = projectDto.RepositoryUrl;
        project.Category = projectDto.Category ?? project.Category;
        project.Technology = projectDto.Technology ?? project.Technology;
        project.Status = projectDto.Status ?? project.Status;
        project.TeamMembers = projectDto.TeamMembers;
        project.Challenges = projectDto.Challenges;
        project.Outcomes = projectDto.Outcomes;
        project.Summary = projectDto.Summary;
        project.IsFeatured = projectDto.IsFeatured;
        project.UpdatedAt = DateTime.UtcNow;
    }

    private static ProjectDto MappingProjectDto(ProjectDto projectDto, Projects project)
    {
        return new ProjectDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            ImageAlt = project.ImageAlt,
            Link = project.Link,
            RepositoryUrl = project.RepositoryUrl,
            Category = project.Category,
            Technology = project.Technology,
            Status = project.Status,
            TeamMembers = project.TeamMembers,
            Challenges = project.Challenges,
            Outcomes = project.Outcomes,
            Summary = project.Summary,
            IsFeatured = project.IsFeatured,
            ImageFile = ConvertImagePathToFile(project.Image),
        };
    }

    private static FormFile? ConvertImagePathToFile(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
        {
            return null;
        }

        // Prepend "wwwroot" to the image path
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", imagePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar))
            .Replace(Path.DirectorySeparatorChar.ToString() + Path.DirectorySeparatorChar, Path.DirectorySeparatorChar.ToString());

        // Check if the directory exists
        var directory = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(directory))
        {
            throw new DirectoryNotFoundException($"The directory '{directory}' does not exist.");
        }

        // Check if the file exists
        if (!File.Exists(fullPath))
        {
            throw new FileNotFoundException($"The file '{fullPath}' does not exist.");
        }

        var fileStream = new FileStream(fullPath, FileMode.Open, FileAccess.Read);
        return new FormFile(fileStream, 0, fileStream.Length, "file", Path.GetFileName(fullPath))
        {
            Headers = new HeaderDictionary(),
            ContentType = "application/octet-stream"
        };
    }
}
