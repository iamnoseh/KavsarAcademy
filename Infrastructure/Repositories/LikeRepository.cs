using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces.LIke;
using Infrastructure.Services.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Infrastructure.Repositories;

public class LikeRepository(DataContext context) : ILikeRepository
{
    
    public async Task<Like?> GetByIdAsync(int id)
    {
        return await context.Likes.FindAsync(id);
    }

    public async Task<List<Like?>> GetAllAsync()
    {
        return await context.Likes.ToListAsync();
    }

    public async Task AddAsync(Like? like)
    {
        await context.Likes.AddAsync(like);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var like = await GetByIdAsync(id);
        if (like != null)
        {
            context.Likes.Remove(like);
            await context.SaveChangesAsync();
        }
    }

    public async Task<Like?> GetByUserAndNewsAsync(int userId, int newsId)
    {
        
        return await context.Likes
            .FirstOrDefaultAsync(l => l != null && l.UserId == userId && l.NewsId == newsId);
    }
}