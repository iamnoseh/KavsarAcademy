using Domain.Entities;

namespace Infrastructure.Interfaces.Material;

public interface IMaterialRepository
{
    Task<List<Domain.Entities.Material>> GetAll();
    Task<List<Domain.Entities.Material>> GetByCourseId(int courseId);
    Task<Domain.Entities.Material?> GetById(int id);
    Task<int> Create(Domain.Entities.Material material);
    Task<int> Update(Domain.Entities.Material material);
    Task<int> Delete(int id);
} 