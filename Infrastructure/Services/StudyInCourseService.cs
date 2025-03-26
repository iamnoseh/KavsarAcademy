using System.Net;
using Domain.Dtos.StudyInCourse;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces.StudyInCourse;

namespace Infrastructure.Services;

public class StudyInCourseService(
    IStudyInCourseRepository studyInCourseRepository,
    string uploadPath
) : IStudyInCourseService
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif"];
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
    
    public async Task<Response<List<GetStudyInCourseDto>>> GetAllStudyInCourses(string language = "En")
    {
        var studyInCourseType = typeof(StudyInCourse);
        var studyInCourses = await studyInCourseRepository.GetAll();
        
        if (!studyInCourses.Any())
            return new Response<List<GetStudyInCourseDto>>(HttpStatusCode.NotFound, "StudyInCourses not found");
            
        var studyInCoursesDto = studyInCourses.Select(s => new GetStudyInCourseDto
        {
            Id = s.Id,
            CourseId = s.CourseId,
            Title = studyInCourseType.GetProperty("Title" + language)?.GetValue(s)?.ToString(),
            Description = studyInCourseType.GetProperty("Description" + language)?.GetValue(s)?.ToString(),
            ImagePath = s.ImagePath
        }).ToList();
        
        return new Response<List<GetStudyInCourseDto>>(studyInCoursesDto);
    }

    public async Task<Response<List<GetStudyInCourseDto>>> GetStudyInCoursesByCourse(int courseId, string language = "En")
    {
        var studyInCourseType = typeof(StudyInCourse);
        var studyInCourses = await studyInCourseRepository.GetByCourseId(courseId);
        
        if (!studyInCourses.Any())
            return new Response<List<GetStudyInCourseDto>>(HttpStatusCode.NotFound, "StudyInCourses not found");
            
        var studyInCoursesDto = studyInCourses.Select(s => new GetStudyInCourseDto
        {
            Id = s.Id,
            CourseId = s.CourseId,
            Title = studyInCourseType.GetProperty("Title" + language)?.GetValue(s)?.ToString(),
            Description = studyInCourseType.GetProperty("Description" + language)?.GetValue(s)?.ToString(),
            ImagePath = s.ImagePath
        }).ToList();
        
        return new Response<List<GetStudyInCourseDto>>(studyInCoursesDto);
    }

    public async Task<Response<GetStudyInCourseDto>> GetStudyInCourseById(int id, string language = "En")
    {
        var studyInCourseType = typeof(StudyInCourse);
        var studyInCourse = await studyInCourseRepository.GetById(id);
        
        if (studyInCourse == null)
            return new Response<GetStudyInCourseDto>(HttpStatusCode.NotFound, "StudyInCourse not found");
            
        var studyInCourseDto = new GetStudyInCourseDto
        {
            Id = studyInCourse.Id,
            CourseId = studyInCourse.CourseId,
            Title = studyInCourseType.GetProperty("Title" + language)?.GetValue(studyInCourse)?.ToString(),
            Description = studyInCourseType.GetProperty("Description" + language)?.GetValue(studyInCourse)?.ToString(),
            ImagePath = studyInCourse.ImagePath
        };
        
        return new Response<GetStudyInCourseDto>(studyInCourseDto);
    }

    public async Task<Response<string>> CreateStudyInCourse(CreateStudyInCourseDto dto)
    {
        string imagePath = null;
        
        // Если есть изображение, сохраняем его
        if (dto.Image != null && dto.Image.Length > 0)
        {
            if (dto.Image.Length > MaxFileSize)
                return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 10MB");
                
            var fileExtension = Path.GetExtension(dto.Image.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                return new Response<string>(HttpStatusCode.BadRequest,
                    "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");
                    
            var uploadsFolder = Path.Combine(uploadPath, "uploads", "studyincourse");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
                
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(fileStream);
            }
            
            imagePath = $"/uploads/studyincourse/{uniqueFileName}";
        }
        
        var studyInCourse = new StudyInCourse
        {
            CourseId = dto.CourseId,
            TitleTj = dto.TitleTj,
            TitleRu = dto.TitleRu,
            TitleEn = dto.TitleEn,
            DescriptionTj = dto.DescriptionTj,
            DescriptionRu = dto.DescriptionRu,
            DescriptionEn = dto.DescriptionEn,
            ImagePath = imagePath
        };
        
        var result = await studyInCourseRepository.Create(studyInCourse);
        
        return result > 0
            ? new Response<string>(HttpStatusCode.Created, "StudyInCourse created successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> UpdateStudyInCourse(UpdateStudyInCourseDto dto)
    {
        var studyInCourse = await studyInCourseRepository.GetById(dto.Id);
        
        if (studyInCourse == null)
            return new Response<string>(HttpStatusCode.NotFound, "StudyInCourse not found");
            
        studyInCourse.TitleTj = dto.TitleTj;
        studyInCourse.TitleRu = dto.TitleRu;
        studyInCourse.TitleEn = dto.TitleEn;
        studyInCourse.DescriptionTj = dto.DescriptionTj;
        studyInCourse.DescriptionRu = dto.DescriptionRu;
        studyInCourse.DescriptionEn = dto.DescriptionEn;
        studyInCourse.CourseId = dto.CourseId;
        
        // Если есть новое изображение, обновляем его
        if (dto.Image != null && dto.Image.Length > 0)
        {
            if (dto.Image.Length > MaxFileSize)
                return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 10MB");
                
            var fileExtension = Path.GetExtension(dto.Image.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                return new Response<string>(HttpStatusCode.BadRequest,
                    "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");
                    
            var uploadsFolder = Path.Combine(uploadPath, "uploads", "studyincourse");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);
                
            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await dto.Image.CopyToAsync(fileStream);
            }
            
            studyInCourse.ImagePath = $"/uploads/studyincourse/{uniqueFileName}";
        }
        
        var result = await studyInCourseRepository.Update(studyInCourse);
        
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent, "StudyInCourse updated successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> DeleteStudyInCourse(int id)
    {
        var studyInCourse = await studyInCourseRepository.GetById(id);
        
        if (studyInCourse == null)
            return new Response<string>(HttpStatusCode.NotFound, "StudyInCourse not found");
            
        var result = await studyInCourseRepository.Delete(id);
        
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent, "StudyInCourse deleted successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }
} 