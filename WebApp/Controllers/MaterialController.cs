using Domain.Dtos.Material;
using Domain.Responses;
using Infrastructure.Interfaces.Material;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaterialController(IMaterialService service) : ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetMaterialDto>>> GetAllMaterials(string language = "Ru")
    {
        return await service.GetAllMaterials(language);
    }
    
    [HttpGet("course/{courseId}")]
    public async Task<Response<List<GetMaterialDto>>> GetMaterialsByCourse(int courseId, string language = "Ru")
    {
        return await service.GetMaterialsByCourse(courseId, language);
    }
    
    [HttpGet("{id}")]
    public async Task<Response<GetMaterialDto>> GetMaterialById(int id, string language = "Ru")
    {
        return await service.GetMaterialById(id, language);
    }
    
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateMaterial(CreateMaterialDto dto)
    {
        return await service.CreateMaterial(dto);
    }
    
    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateMaterial(UpdateMaterialDto dto)
    {
        return await service.UpdateMaterial(dto);
    }
    
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteMaterial(int id)
    {
        return await service.DeleteMaterial(id);
    }
} 