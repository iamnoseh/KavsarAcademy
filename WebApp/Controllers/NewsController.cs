using Domain.Dtos;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class NewsController(INewsService service) : ControllerBase
{
    [HttpGet]
    public async Task<Response<List<GetNewsDto>>> GetNews(string language = "En")
    {
        return await service.GetNewsAsync(language);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateNews([FromForm]CreateNewsDto newsDto)
    {
        return await service.CreateNewsAsync(newsDto);
    }
    
    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateNews([FromForm]UpdateNewsDto newsDto)
    {
        return await service.UpdateNewsAsync(newsDto);
    }
    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteNews(int id)
    {
        return await service.DeleteNewsAsync(id);
    }

    [HttpGet("id")]
    public async Task<Response<GetNewsDto>> GetNews(int id,string language = "En")
    {
        return await service.GetNewsByIdAsync(id, language);
    }
    
}