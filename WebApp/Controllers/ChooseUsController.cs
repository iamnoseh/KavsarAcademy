using Domain.Dtos;
using Infrastructure.Interfaces;
using Infrastructure.Responses;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChooseUsController (IChooseUsService service) : Controller
{
    [HttpGet]
    public async Task<Response<List<GetChooseUsDto>>> GetChooseUs([FromQuery] string language)
    {
        return await service.GetChooseUsAsync(language);
    }
    [HttpGet("{id}")]
    public async Task<Response<GetChooseUsDto>> GetChooseUsById(int id,[FromQuery]string language)
    {
        return await service.GetChooseUsByIdAsync(id, language);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateChooseUs([FromForm] CreateChooseUsDto createChooseUsDto)
    {
        return await service.CreateChooseUsAsync(createChooseUsDto);
    }
    [HttpPut("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateChooseUs([FromForm] UpdateChooseUsDto chooseUsDto)
    {
        return await service.UpdateChooseUsAsync(chooseUsDto);
    }
    [HttpDelete("{id}")]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteChooseUs(int id)
    {
        return await service.DeleteChooseUsAsync(id);
    }
    
}