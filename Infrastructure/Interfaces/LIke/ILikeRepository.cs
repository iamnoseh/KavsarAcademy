using Domain.Entities;

namespace Infrastructure.Interfaces.LIke;

public interface ILikeRepository
{
    Task<Like?> GetByIdAsync(int id);
    Task<List<Like?>> GetAllAsync();
    Task AddAsync(Like? like);
    Task DeleteAsync(int id);
    Task<Like?> GetByUserAndNewsAsync(int userId, int newsId);
}