using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class NewsRepository(DataContext context) : INewsRepository
{
    public async Task<List<News?>> GetAllNews()
    {
        return await context.News.Include(n => n.Likes).ToListAsync();

    }
    
    public async Task<News?> GetNewsById(int id)
    {
        return await context.News.Include(n => n.Likes).FirstOrDefaultAsync(n => n != null && n.Id == id);
    }

    public async Task<int> CreateNews(News? news)
    {
        if (news != null) await context.News.AddAsync(news);
        return await context.SaveChangesAsync();
    }

    public async Task<int> UpdateNews(News? news)
    {
        context.News.Update(news);
        return await context.SaveChangesAsync();
    }

    public async Task<int> DeleteNews(News? news)
    {
        context.News.Remove(news);
        return await context.SaveChangesAsync();
    }
}