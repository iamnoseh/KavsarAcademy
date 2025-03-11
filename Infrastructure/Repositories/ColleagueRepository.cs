using Domain.Entities;
using Infrastructure.Data;
using Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public class ColleagueRepository (DataContext context) : IColleagueRepository
{
    public async Task<List<Colleague>> GetAll()
    {
        return await context.Colleagues.Where(x => x.IsDeleted == false).ToListAsync();
    }

    public async Task<Colleague?> GetById(int id)
    {
        var res  = await context.Colleagues.Where(x => x.Id == id && x.IsDeleted == false).FirstOrDefaultAsync();
        return res;
    }

    public async Task<int> Create(Colleague colleague)
    {
        await context.Colleagues.AddAsync(colleague);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Update(Colleague colleague)
    {
        context.Update(colleague);
        return await context.SaveChangesAsync();
    }

    public async Task<int> Delete(Colleague colleague)
    {
        var deleteColleague = await context.Colleagues.FirstOrDefaultAsync(x => x.Id == colleague.Id);
        if (deleteColleague == null) return 0;
        deleteColleague.IsDeleted = true;
        deleteColleague.DeletedAt = DateTime.UtcNow;
        return await context.SaveChangesAsync();
    }
}