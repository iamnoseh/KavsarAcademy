using Domain.Entities;

namespace Infrastructure.Interfaces;

public interface IColleagueRepository
{
    Task<List<Colleague>> GetAll();
    Task<Colleague?> GetById(int id);
    Task<int> Create(Colleague colleague);
    Task<int> Update(Colleague colleague);
    Task<int> Delete(Colleague colleague);
}