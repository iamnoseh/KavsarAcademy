using Domain.Dtos.Colleague;
using Domain.Dtos.Material;
using Domain.Dtos.StudyInCourse;

namespace Domain.Dtos;

public class GetCourseDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal Price { get; set; }
    public int Duration { get; set; }
    public string ImagePath { get; set; }
    
    // Полная информация о преподавателе
    public GetColleague? Colleague { get; set; }
    
    // Списки материалов и StudyInCourse
    public List<GetMaterialDto>? Materials { get; set; }
    public List<GetStudyInCourseDto>? StudyInCourses { get; set; }
}