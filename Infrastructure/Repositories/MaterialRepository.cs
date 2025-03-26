using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces.Material;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class MaterialRepository(DataContext context) : IMaterialRepository
{
    public async Task<List<Material>> GetAll()
    {
        return await context.Materials.ToListAsync();
    }

    public async Task<List<Material>> GetByCourseId(int courseId)
    {
        return await context.Materials
            .Where(m => m.CourseId == courseId)
            .ToListAsync();
    }

    public async Task<Material?> GetById(int id)
    {
        return await context.Materials
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<int> Create(Material material)
    {
        await context.Materials.AddAsync(material);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Update(Material material)
    {
        context.Materials.Update(material);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(int id)
    {
        var material = await GetById(id);
        if (material == null) return 0;
        
        context.Materials.Remove(material);
        return await context.SaveChangesAsync();
    }
} 