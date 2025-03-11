using Domain.Dtos;
using Infrastructure.Interfaces.LIke;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LikeController(ILikeService likeService) : ControllerBase
{
    [HttpPost("toggle")]
    public async Task<IActionResult> ToggleLike([FromBody] CreateLikeDto dto)
    {
        if (dto.NewsId == null)
        {
            return BadRequest("News ID is required");
        }

        await likeService.ToggleLikeAsync(dto.UserId, dto.NewsId.Value);
        return Ok();
    }

    [HttpGet("count/{newsId}")]
    public async Task<IActionResult> GetLikeCount(int newsId)
    {
        var count = await likeService.GetLikeCountAsync(newsId);
        return Ok(count);
    }
}