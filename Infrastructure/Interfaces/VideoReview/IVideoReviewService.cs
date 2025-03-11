using Domain.Dtos.VideoReview;
using Infrastructure.Responses;

namespace Infrastructure.Interfaces.VideoReview;

public interface IVideoReviewService
{
    Task<Response<List<GetVideoReview>>> GetVideoReviewsAsync(string language = "En");
    Task<Response<GetVideoReview>> GetVideoReviewByIdAsync(int requestId,string language = "En");
    Task<Response<string>> CreateReviewVideo(CreateVideoReview request);
    Task<Response<string>> UpdateReviewVideo(UpdateVideoReview request);
    Task<Response<string>> DeleteReviewVideo(int requestId);
}