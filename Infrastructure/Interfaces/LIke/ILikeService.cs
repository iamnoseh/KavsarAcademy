namespace Infrastructure.Interfaces.LIke;

public interface ILikeService
{
    Task ToggleLikeAsync(int userId, int newsId);
    Task<int> GetLikeCountAsync(int newsId);
}