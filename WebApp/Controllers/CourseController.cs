using Domain.Dtos;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController(ICourseService service) : Controller
{
    [HttpGet]
    public async Task<Response<List<GetCourseDto>>> GetCourses(string language = "Ru")
    {
        return await service.GetCoursesAsync(language);
    }

    [HttpGet("{id}")]
    public async Task<Response<GetCourseDto>> GetCourseById(int id,string language = "Ru")
    {
        return await service.GetCourseByIdAsync(id, language);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateCourse([FromForm]CreateCourseDto courseDto)
    {
        return await service.CreateCourseAsync(courseDto);
    }
    
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateCourse([FromForm]UpdateCourseDto courseDto)
    {
        return await service.UpdateCourseAsync(courseDto);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteCourse(int id)
    {
        return await service.DeleteCourseAsync(id);
    }
}