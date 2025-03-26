namespace Domain.Dtos.StudyInCourse;

public class GetStudyInCourseDto
{
    public int Id { get; set; }
    public int CourseId { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ImagePath { get; set; }
} 