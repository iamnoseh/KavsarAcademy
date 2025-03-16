using Domain.Dtos;
using Domain.Dtos.Gallery;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class GalleryController (IGalleryService service) : Controller
{
    [HttpGet]
    public async Task<Response<List<GetMediaDto>>> GetAllMedias() => 
        await service.GetMediasAsync();

    [HttpGet("{id}")]
    public async Task<Response<GetMediaDto>> GetMedia(int id) =>
        await service.GetMediaAsync(id);

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateMedia(CreateMediaDto createMediaDto) => 
        await service.CreateMediaAsync(createMediaDto);

    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> EditMedia(EditMediaDto editMediaDto) => 
        await service.EditMediaAsync(editMediaDto);

    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteMedia(int id) =>
        await service.DeleteMediaAsync(id);
    
}