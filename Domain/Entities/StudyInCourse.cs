namespace Domain.Entities;
using Microsoft.AspNetCore.Http;

public class StudyInCourse
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string? ImagePath { get; set; }
    
    // Заголовки на трёх языках
    public string? TitleTj { get; set; }
    public string? TitleRu { get; set; }
    public string? TitleEn { get; set; }
    
    // Описания на трёх языках
    public string? DescriptionTj { get; set; }
    public string? DescriptionRu { get; set; }
    public string? DescriptionEn { get; set; }
    
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public IFormFile? Image { get; set; }
    
    // Навигационное свойство для курса
    public Course Course { get; set; }
}

