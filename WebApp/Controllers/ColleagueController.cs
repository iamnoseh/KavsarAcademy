using Domain.Dtos.Colleague;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ColleagueController (IColleagueService service) : ControllerBase
{
    [HttpGet("colleagueWithIcons")]
    public async Task<Response<List<GetColleagueWhitKnowingIcons>>> GetColleagueWithIcons(string language = "Ru") => 
        await service.GetColleaguesWithKnowingIcons(language);
    
    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateColleague(CreateColleague request) =>
        await service.CreateColleague(request);
   
    [HttpGet("{id}")]
    public async Task<Response<GetColleague>> GetColleagueId(int id,string language = "Ru") => 
        await service.GetColleagueById(id,language);
    
    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateColleague(EditColleague request) => 
        await service.EditColleague(request);
    
    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteColleague(int id) => await service.DeleteColleague(id);
    
    [HttpGet("colleagueWithIcon/{id}")] 
    public async Task<Response<GetColleagueWhitKnowingIcons>> GetColleagueBy(int id , string language = "Ru") => 
        await service.GetColleagueWithKnowingIcon(id,language);
    
}