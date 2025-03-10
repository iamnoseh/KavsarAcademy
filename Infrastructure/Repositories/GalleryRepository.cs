using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Gallery;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class GalleryRepository (DataContext context) : IGalleryRepository
{
    public async Task<List<Gallery>> GetAll()
    {
        return await context.Galleries.Where(x => x.IsDeleted == false).ToListAsync();
    }

    public async Task<Gallery?> GetById(int id)
    {
        return await context.Galleries.Where(x => x.Id == id).FirstOrDefaultAsync();
    }

    public async Task<int> CreateMedia(Gallery request)
    {
        await context.Galleries.AddAsync(request);
        return await context.SaveChangesAsync();
    }

    public async Task<int> EditMedia(Gallery request)
    {
        var media = await context.Galleries.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
        if (media != null)
        {
            media.IsDeleted = request.IsDeleted;
            media.MediaUrl = request.MediaUrl;
            media.Id = request.Id;
            media.UpdatedAt = DateTime.UtcNow;
            media.CreatedAt = request.CreatedAt;
            media.DeletedAt = request.DeletedAt;
        }

        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteMedia(Gallery request)
    {
        var media = await context.Galleries.Where(x => x.Id == request.Id).FirstOrDefaultAsync();
        if (media != null)
        {
            media.IsDeleted = true;
            media.DeletedAt = DateTime.UtcNow;
        }

        return await context.SaveChangesAsync();
    }
}