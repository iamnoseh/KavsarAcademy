using Domain.Dtos.BannerDto;
using Domain.Responses;
using Infrastructure.Interfaces.IServices;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BannerController(IBannerService bannerService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAllBanners([FromQuery] string language = "Ru")
    {
        var response = await bannerService.GetAllBanners(language);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<Task<Response<GetBannerDto>>> GetBannerById(int id, [FromQuery] string language = "Ru")
    {
        return  bannerService.GetBannerById(id, language);
        
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> CreateBanner([FromForm] CreateBannerDto dto)
    {
        var response = await bannerService.CreateBanner(dto);
        return Ok(response);
    }

    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> UpdateBanner([FromForm] UpdateBannerDto dto)
    {
        var response = await bannerService.UpdateBanner(dto);
        return Ok(response);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<IActionResult> DeleteBanner(int id)
    {
        var response = await bannerService.DeleteBanner(id);
        return Ok(response);
    }
}