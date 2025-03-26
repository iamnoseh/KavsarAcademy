using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.StudyInCourse;

public class CreateStudyInCourseDto
{
    public int CourseId { get; set; }
    
    // Заголовки на трёх языках
    public string? TitleTj { get; set; }
    public string? TitleRu { get; set; }
    public string? TitleEn { get; set; }
    
    // Описания на трёх языках
    public string? DescriptionTj { get; set; }
    public string? DescriptionRu { get; set; }
    public string? DescriptionEn { get; set; }
    
    public IFormFile? Image { get; set; }
} 