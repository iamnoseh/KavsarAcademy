using Domain.Entities;
using Infrastructure.Interfaces.LIke;
using Infrastructure.Services.Memory;

namespace Infrastructure.Services;

public class LikeService (ILikeRepository likeRepository,IRedisMemoryCache memoryCache) : ILikeService
{
    private const string Key = "news";
    public async Task ToggleLikeAsync(int userId, int newsId)
    {
        var existingLike = await likeRepository.GetByUserAndNewsAsync(userId, newsId);
        if (existingLike != null)
        {
            //agar user like monda boshad onro nest mekunem
            await memoryCache.RemoveDataAsync(Key);
            await likeRepository.DeleteAsync(existingLike.Id);
        }
        else
        {
            // agar namondaboshad +1like mekunem
            var newLike = new Like { UserId = userId, NewsId = newsId };
            await memoryCache.RemoveDataAsync(Key);
            await likeRepository.AddAsync(newLike);
        }
    }

    public async Task<int> GetLikeCountAsync(int newsId)
    {
        var likes = await likeRepository.GetAllAsync();
        return likes.Count(l => l?.NewsId == newsId);
    }
}