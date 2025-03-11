using Domain.Dtos.VideoReview;
using Infrastructure.Interfaces.VideoReview;
using Infrastructure.Responses;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StackExchange.Redis;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class VideoReviewController(IVideoReviewService service) : Controller
{
    [HttpGet]
    public async Task<Response<List<GetVideoReview>>> GetVideoReview(string language = "En") => 
        await service.GetVideoReviewsAsync(language);
    
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateVideoReview(CreateVideoReview review) => 
        await service.CreateReviewVideo(review);
    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateVideoReview(UpdateVideoReview review) => 
        await service.UpdateReviewVideo(review);

    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteVideoReview(int id) => 
        await service.DeleteReviewVideo(id);

    [HttpGet("id")]
    public async Task<Response<GetVideoReview>> GetVideoReviewById(int id, string language = "En") =>
        await service.GetVideoReviewByIdAsync(id, language);
}