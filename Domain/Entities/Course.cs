using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Filters;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Course : BaseEntity
{
    [Required]
    public string NameTj { get; set; }
    public string NameRu { get; set; }
    public string NameEn { get; set; }
    public string DescriptionTj { get; set; }
    public string DescriptionRu { get; set; }
    public string DescriptionEn { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public string ImagePath { get; set; }
    
    [NotMapped]
    public IFormFile Image { get; set; }
    
    // Связь с преподавателем (один преподаватель для курса)
    public int? ColleagueId { get; set; }
    public Colleague Colleague { get; set; }
    
    // Связь с материалами курса (множество материалов)
    public List<Material> Materials { get; set; }
    
    // Связь с StudyInCourse (множество элементов)
    public List<StudyInCourse> StudyInCourses { get; set; }
}