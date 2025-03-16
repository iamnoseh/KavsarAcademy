using Domain.Dtos;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Seed;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers;
[ApiController]
[Route("api/[controller]")]
public class BranchController (IBranchService service) : Controller
{
    [HttpGet]
    public async Task<Response<List<GetBranchDto>>> GetBranches(string language = "Ru")
    {
        return await service.GetAllBranchesAsync(language);
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetBranchById(int id, [FromQuery] string language = "Ru")
    {
        var response = await service.GetBranchByIdAsync(id, language);
        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> CreateBranch([FromForm]CreateBranchDto branchDto)
    {
        return await service.CreateBranchAsync(branchDto);
    }

    [HttpDelete]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> DeleteBranch(int id)
    {
        return await service.DeleteBranchAsync(id);
    }

    [HttpPut]
    [Authorize(Roles = Roles.Admin)]
    public async Task<Response<string>> UpdateBranch(UpdateBranchDto branchDto)
    {
        return await service.UpdateBranchAsync(branchDto);
    }
    
}