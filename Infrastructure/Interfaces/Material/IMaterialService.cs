using Domain.Dtos.Material;
using Domain.Responses;

namespace Infrastructure.Interfaces.Material;

public interface IMaterialService
{
    Task<Response<List<GetMaterialDto>>> GetAllMaterials(string language = "En");
    Task<Response<List<GetMaterialDto>>> GetMaterialsByCourse(int courseId, string language = "En");
    Task<Response<GetMaterialDto>> GetMaterialById(int id, string language = "En");
    Task<Response<string>> CreateMaterial(CreateMaterialDto dto);
    Task<Response<string>> UpdateMaterial(UpdateMaterialDto dto);
    Task<Response<string>> DeleteMaterial(int id);
} 