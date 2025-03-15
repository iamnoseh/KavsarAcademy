using System.Net;
using Domain.Dtos.VideoReview;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Data;
using Infrastructure.Interfaces.VideoReview;
using Infrastructure.Services.Memory;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public class VideoReviewService(
    DataContext context,
    IRedisMemoryCache memoryCache,
    string uploadPath): IVideoReviewService
{
    const string Key = "VideoReview";
    private const long MaxFileSize = 250 * 1024 * 1024; // 250Mb 
    public async Task<Response<List<GetVideoReview>>> GetVideoReviewsAsync(string language = "En")
    {
        var typeReview = typeof(VideoReview);
        var reviews = await memoryCache.GetDataAsync<List<GetVideoReview>>(Key);
        if (reviews is null)
        {
            var videoReviews = await context.VideoReviews.ToListAsync();
            if (!videoReviews.Any()) 
                return new Response<List<GetVideoReview>>(HttpStatusCode.NotFound,"Video reviews not found");
            reviews = videoReviews.Select(x => new GetVideoReview
            {
                ReviewerName = typeReview.GetProperty("ReviewerName" + language)?.GetValue(x).ToString(),
                VideoReviewFile = x.ReviewPath,
            }).ToList();
            await memoryCache.SetDataAsync(Key, reviews,1000);
        }
        return new Response<List<GetVideoReview>>(reviews);
    }

    public async Task<Response<GetVideoReview>> GetVideoReviewByIdAsync(int requestId, string language = "En")
    {
        var typeReview = typeof(VideoReview);
        var review = await context.VideoReviews.FirstOrDefaultAsync(x => x.Id == requestId);
        if (review is null) 
            return new Response<GetVideoReview>(HttpStatusCode.NotFound,"Video review not found");
        var dto = new GetVideoReview
        {
            ReviewerName = typeReview.GetProperty("ReviewerName" + language)?.GetValue(review).ToString(),
            VideoReviewFile = review.ReviewPath,
        };
        return new Response<GetVideoReview>(dto);
    }

    public async Task<Response<string>> CreateReviewVideo(CreateVideoReview request)
    {
        if (request.VideoReviewFile != null && request.VideoReviewFile.Length == 0)
            return new Response<string>(HttpStatusCode.BadRequest, "VideoReviews file is required");

        if (request.VideoReviewFile != null && request.VideoReviewFile.Length > MaxFileSize)
            return new Response<string>(HttpStatusCode.BadRequest, "VideoReviews file size must be less than 250Mb");

        var fileExtension = Path.GetExtension(request.VideoReviewFile?.FileName)?.ToLower();
        
        var uploadsFolder = Path.Combine(uploadPath, "uploads", "VideoReviews");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            if (request.VideoReviewFile != null) await request.VideoReviewFile.CopyToAsync(fileStream);
        }

        var media = new VideoReview()
        {
            CreatedAt = DateTime.UtcNow,
            ReviewPath = $"/uploads/VideoReviews/{uniqueFileName}",
            ReviewerNameRu = request.ReviewerNameRu,
            ReviewerNameTj = request.ReviewerNameTj,
            ReviewerNameEn = request.ReviewerNameEn,
            UpdatedAt = DateTime.UtcNow,
        };
        await context.VideoReviews.AddAsync(media);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>(HttpStatusCode.OK, "Video review saved")
            : new Response<string>(HttpStatusCode.InternalServerError, "Video review could not be saved");
    }

    public async Task<Response<string>> UpdateReviewVideo(UpdateVideoReview request)
    {
        var media = await context.VideoReviews.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (media == null)
            return new Response<string>(HttpStatusCode.NotFound, "Media not found");
        media.Id = request.Id;
        media.UpdatedAt = DateTime.UtcNow;
        media.ReviewerNameRu = request.ReviewerNameRu;
        media.ReviewerNameTj = request.ReviewerNameTj;
        media.ReviewerNameEn = request.ReviewerNameEn;
        {
            if (request.VideoReviewFile != null && request.VideoReviewFile.Length > MaxFileSize)
                return new Response<string>(HttpStatusCode.BadRequest,
                    "Image file size must be less than 250MB");

            var fileExtension = Path.GetExtension(request.VideoReviewFile?.FileName)?.ToLower();

            var uploadsFolder = Path.Combine(uploadPath, "uploads", "VideoReviews");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.VideoReviewFile.CopyToAsync(fileStream);
            }

            media.ReviewPath = $"/uploads/VideoReviews/{uniqueFileName}";
        }
        var res = await context.SaveChangesAsync();
        return res > 0
            ? new Response<string>(HttpStatusCode.OK, "VideoReview Updated Successfully")
            : new Response<string>(HttpStatusCode.InternalServerError, "Video review could not be updated");
    }

    public async Task<Response<string>> DeleteReviewVideo(int requestId)
    {
        var deleteRequest = await context.VideoReviews.FirstOrDefaultAsync(x => x.Id == requestId);
        if (deleteRequest is null) return new Response<string>(HttpStatusCode.NotFound,"Video review not found");
        context.VideoReviews.Remove(deleteRequest);
        var result = await context.SaveChangesAsync();
        return result > 0
            ? new Response<string>(HttpStatusCode.OK, "Video review deleted")
            : new Response<string>(HttpStatusCode.InternalServerError, "Video review could not be deleted");
    }
}