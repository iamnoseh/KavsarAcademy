using System.Net;
using Domain.Dtos.Material;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces.Material;

namespace Infrastructure.Services;

public class MaterialService(IMaterialRepository materialRepository) : IMaterialService
{
    public async Task<Response<List<GetMaterialDto>>> GetAllMaterials(string language = "En")
    {
        var materialType = typeof(Material);
        var materials = await materialRepository.GetAll();
        
        if (!materials.Any())
            return new Response<List<GetMaterialDto>>(HttpStatusCode.NotFound, "Materials not found");
            
        var materialsDto = materials.Select(m => new GetMaterialDto
        {
            Id = m.Id,
            Title = materialType.GetProperty("Title" + language)?.GetValue(m)?.ToString(),
            Description = materialType.GetProperty("Description" + language)?.GetValue(m)?.ToString(),
            CourseId = m.CourseId
        }).ToList();
        
        return new Response<List<GetMaterialDto>>(materialsDto);
    }

    public async Task<Response<List<GetMaterialDto>>> GetMaterialsByCourse(int courseId, string language = "En")
    {
        var materialType = typeof(Material);
        var materials = await materialRepository.GetByCourseId(courseId);
        
        if (!materials.Any())
            return new Response<List<GetMaterialDto>>(HttpStatusCode.NotFound, "Materials not found");
            
        var materialsDto = materials.Select(m => new GetMaterialDto
        {
            Id = m.Id,
            Title = materialType.GetProperty("Title" + language)?.GetValue(m)?.ToString(),
            Description = materialType.GetProperty("Description" + language)?.GetValue(m)?.ToString(),
            CourseId = m.CourseId
        }).ToList();
        
        return new Response<List<GetMaterialDto>>(materialsDto);
    }

    public async Task<Response<GetMaterialDto>> GetMaterialById(int id, string language = "En")
    {
        var materialType = typeof(Material);
        var material = await materialRepository.GetById(id);
        
        if (material == null)
            return new Response<GetMaterialDto>(HttpStatusCode.NotFound, "Material not found");
            
        var materialDto = new GetMaterialDto
        {
            Id = material.Id,
            Title = materialType.GetProperty("Title" + language)?.GetValue(material)?.ToString(),
            Description = materialType.GetProperty("Description" + language)?.GetValue(material)?.ToString(),
            CourseId = material.CourseId
        };
        
        return new Response<GetMaterialDto>(materialDto);
    }

    public async Task<Response<string>> CreateMaterial(CreateMaterialDto dto)
    {
        var material = new Material
        {
            TitleTj = dto.TitleTj,
            TitleRu = dto.TitleRu,
            TitleEn = dto.TitleEn,
            DescriptionTj = dto.DescriptionTj,
            DescriptionRu = dto.DescriptionRu,
            DescriptionEn = dto.DescriptionEn,
            CourseId = dto.CourseId
        };
        
        var result = await materialRepository.Create(material);
        
        return result > 0
            ? new Response<string>(HttpStatusCode.Created, "Material created successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> UpdateMaterial(UpdateMaterialDto dto)
    {
        var material = await materialRepository.GetById(dto.Id);
        
        if (material == null)
            return new Response<string>(HttpStatusCode.NotFound, "Material not found");
            
        material.TitleTj = dto.TitleTj;
        material.TitleRu = dto.TitleRu;
        material.TitleEn = dto.TitleEn;
        material.DescriptionTj = dto.DescriptionTj;
        material.DescriptionRu = dto.DescriptionRu;
        material.DescriptionEn = dto.DescriptionEn;
        material.CourseId = dto.CourseId;
        
        var result = await materialRepository.Update(material);
        
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent, "Material updated successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> DeleteMaterial(int id)
    {
        var material = await materialRepository.GetById(id);
        
        if (material == null)
            return new Response<string>(HttpStatusCode.NotFound, "Material not found");
            
        var result = await materialRepository.Delete(id);
        
        return result > 0
            ? new Response<string>(HttpStatusCode.NoContent, "Material deleted successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }
} 