using Domain.Dtos.StudyInCourse;
using Domain.Responses;

namespace Infrastructure.Interfaces.StudyInCourse;

public interface IStudyInCourseService
{
    Task<Response<List<GetStudyInCourseDto>>> GetAllStudyInCourses(string language = "En");
    Task<Response<List<GetStudyInCourseDto>>> GetStudyInCoursesByCourse(int courseId, string language = "En");
    Task<Response<GetStudyInCourseDto>> GetStudyInCourseById(int id, string language = "En");
    Task<Response<string>> CreateStudyInCourse(CreateStudyInCourseDto dto);
    Task<Response<string>> UpdateStudyInCourse(UpdateStudyInCourseDto dto);
    Task<Response<string>> DeleteStudyInCourse(int id);
} 