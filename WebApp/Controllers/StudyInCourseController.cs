using Domain.Dtos.StudyInCourse;
using Domain.Responses;
using Infrastructure.Interfaces.StudyInCourse;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudyInCourseController(IStudyInCourseService service) : ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetStudyInCourseDto>>> GetAllStudyInCourses(string language = "Ru")
    {
        return await service.GetAllStudyInCourses(language);
    }
    
    [HttpGet("course/{courseId}")]
    public async Task<Response<List<GetStudyInCourseDto>>> GetStudyInCoursesByCourse(int courseId, string language = "Ru")
    {
        return await service.GetStudyInCoursesByCourse(courseId, language);
    }
    
    [HttpGet("{id}")]
    public async Task<Response<GetStudyInCourseDto>> GetStudyInCourseById(int id, string language = "Ru")
    {
        return await service.GetStudyInCourseById(id, language);
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateStudyInCourse([FromForm] CreateStudyInCourseDto dto)
    {
        return await service.CreateStudyInCourse(dto);
    }
    
    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateStudyInCourse([FromForm] UpdateStudyInCourseDto dto)
    {
        return await service.UpdateStudyInCourse(dto);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteStudyInCourse(int id)
    {
        return await service.DeleteStudyInCourse(id);
    }
} 