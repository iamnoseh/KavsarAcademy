using System.Net;
using Domain.Dtos;
using Domain.Dtos.Colleague;
using Domain.Dtos.Material;
using Domain.Dtos.StudyInCourse;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class CourseService(
    ICourseRepository courseRepository,
    string uploadPath) : ICourseService
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private const long MaxFileSize = 10 * 1024 * 1024;

    public async Task<Response<List<GetCourseDto>>> GetCoursesAsync(string language = "En")
    {
        var courseType = typeof(Course);
        var colleagueType = typeof(Colleague);
        var materialType = typeof(Material);
        var studyType = typeof(StudyInCourse);

       
        var courses = await courseRepository.GetCoursesWithDetails();

        if (courses == null || !courses.Any())
            return new Response<List<GetCourseDto>>(HttpStatusCode.NotFound, "Courses not found");

        var courseDtos = courses.Select(course => new GetCourseDto
        {
            Id = course.Id,
            Name = courseType.GetProperty("Name" + language)?.GetValue(course)?.ToString(),
            Description = courseType.GetProperty("Description" + language)?.GetValue(course)?.ToString(),
            Duration = course.Duration,
            Price = course.Price,
            ImagePath = course.ImagePath,
            Colleague = course.Colleague != null
                ? new GetColleague
                {
                    Id = course.Colleague.Id,
                    FirstName = colleagueType.GetProperty("FirstName" + language)?.GetValue(course.Colleague)
                        ?.ToString(),
                    LastName = colleagueType.GetProperty("LastName" + language)?.GetValue(course.Colleague)?.ToString(),
                    About = colleagueType.GetProperty("Aboute" + language)?.GetValue(course.Colleague)?.ToString(),
                    ProfileImage = course.Colleague.ImagePath
                }
                : null,
            Materials = course.Materials != null
                ? course.Materials.Select(m => new GetMaterialDto
                {
                    Id = m.Id,
                    Title = materialType.GetProperty("Title" + language)?.GetValue(m)?.ToString(),
                    Description = materialType.GetProperty("Description" + language)?.GetValue(m)?.ToString(),
                    CourseId = m.CourseId
                }).ToList()
                : new List<GetMaterialDto>(),
            StudyInCourses = course.StudyInCourses != null
                ? course.StudyInCourses.Select(s => new GetStudyInCourseDto
                {
                    Id = s.Id,
                    CourseId = s.CourseId,
                    Title = studyType.GetProperty("Title" + language)?.GetValue(s)?.ToString(),
                    Description = studyType.GetProperty("Description" + language)?.GetValue(s)?.ToString(),
                    ImagePath = s.ImagePath
                }).ToList()
                : new List<GetStudyInCourseDto>()
        }).ToList();

        return new Response<List<GetCourseDto>>(courseDtos);
    }

   
    public async Task<Response<GetCourseDto>> GetCourseByIdAsync(int courseId, string language = "En")
    {
        var courseType = typeof(Course);
        var colleagueType = typeof(Colleague);
        var materialType = typeof(Material);
        var studyType = typeof(StudyInCourse);

        var course = await courseRepository.GetCourseWithDetailsById(courseId);

        if (course == null)
            return new Response<GetCourseDto>(HttpStatusCode.NotFound, "Course not found");

        var courseDto = new GetCourseDto
        {
            Id = course.Id,
            Name = courseType.GetProperty("Name" + language)?.GetValue(course)?.ToString(),
            Description = courseType.GetProperty("Description" + language)?.GetValue(course)?.ToString(),
            Duration = course.Duration,
            Price = course.Price,
            ImagePath = course.ImagePath,
            Colleague = course.Colleague != null
                ? new GetColleague
                {
                    Id = course.Colleague.Id,
                    FirstName = colleagueType.GetProperty("FirstName" + language).GetValue(course.Colleague)
                        ?.ToString(),
                    LastName = colleagueType.GetProperty("LastName" + language).GetValue(course.Colleague)?.ToString(),
                    About = colleagueType.GetProperty("Aboute" + language).GetValue(course.Colleague)?.ToString(),
                    ProfileImage = course.Colleague.ImagePath
                }
                : null,
            Materials = course.Materials != null
                ? course.Materials.Select(m => new GetMaterialDto
                {
                    Id = m.Id,
                    Title = materialType.GetProperty("Title" + language)?.GetValue(m)?.ToString(),
                    Description = materialType.GetProperty("Description" + language)?.GetValue(m)?.ToString(),
                    CourseId = m.CourseId
                }).ToList()
                : new List<GetMaterialDto>(),
            StudyInCourses = course.StudyInCourses != null
                ? course.StudyInCourses.Select(s => new GetStudyInCourseDto
                {
                    Id = s.Id,
                    CourseId = s.CourseId,
                    Title = studyType.GetProperty("Title" + language)?.GetValue(s)?.ToString(),
                    Description = studyType.GetProperty("Description" + language)?.GetValue(s)?.ToString(),
                    ImagePath = s.ImagePath
                }).ToList()
                : new List<GetStudyInCourseDto>()
        };

        return new Response<GetCourseDto>(courseDto);
    }

    public async Task<Response<string>> CreateCourseAsync(CreateCourseDto courseDto)
    {
        if (courseDto.Image.Length == 0)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file is required");

        if (courseDto.Image.Length > MaxFileSize)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 10MB");

        var fileExtension = Path.GetExtension(courseDto.Image.FileName).ToLower();
        if (!_allowedExtensions.Contains(fileExtension))
            return new Response<string>(HttpStatusCode.BadRequest,
                "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");


        var uploadsFolder = Path.Combine(uploadPath, "uploads", "course");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await courseDto.Image.CopyToAsync(fileStream);
        }

        var course = new Course
        {
            NameTj = courseDto.NameTj,
            DescriptionTj = courseDto.DescriptionTj,
            NameRu = courseDto.NameRu,
            DescriptionRu = courseDto.DescriptionRu,
            NameEn = courseDto.NameEn,
            DescriptionEn = courseDto.DescriptionEn,
            Duration = courseDto.Duration,
            Price = courseDto.Price,
            CreatedAt = DateTime.UtcNow,
            ImagePath = $"/uploads/course/{uniqueFileName}",
            ColleagueId = courseDto.ColleagueId
        };
        int res = await courseRepository.Create(course);
        return res > 0
            ? new Response<string>(HttpStatusCode.Created, "Course created successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> UpdateCourseAsync(UpdateCourseDto courseDto)
    {
        var banner = await courseRepository.GetById(courseDto.Id);
        if (banner == null)
            return new Response<string>(HttpStatusCode.NotFound, "Course not found");

        banner.NameTj = courseDto.NameTj;
        banner.NameRu = courseDto.NameRu;
        banner.NameEn = courseDto.NameEn;
        banner.DescriptionTj = courseDto.DescriptionTj;
        banner.DescriptionRu = courseDto.DescriptionRu;
        banner.DescriptionEn = courseDto.DescriptionEn;
        banner.UpdatedAt = DateTime.UtcNow;

        {
            if (courseDto.Image.Length > MaxFileSize)
                return new Response<string>(HttpStatusCode.BadRequest,
                    "Image file size must be less than 10MB");

            var fileExtension = Path.GetExtension(courseDto.Image.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                return new Response<string>(HttpStatusCode.BadRequest,
                    "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");

            var uploadsFolder = Path.Combine(uploadPath, "uploads", "course");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await courseDto.Image.CopyToAsync(fileStream);
            }

            banner.ImagePath = $"/uploads/course/{uniqueFileName}";
        }
        var result = await courseRepository.Update(banner);
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent, "Course updated successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> DeleteCourseAsync(int courseId)
    {
        var course = await courseRepository.GetById(courseId);
        if (course == null)
            return new Response<string>(HttpStatusCode.NotFound, "Course not found");
        var result = await courseRepository.Delete(course);
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent, "Course deleted successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }
}