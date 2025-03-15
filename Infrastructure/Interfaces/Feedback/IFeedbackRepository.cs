using Domain.Entities;
using Domain.Filters;

namespace Infrastructure.Interfaces;

public interface IFeedbackRepository
{
    Task<List<Feedback?>> GetAll(BaseFilter filter);
    Task<Feedback?> GetById(int id);
    Task<int> Create(Feedback? feedback);
    Task<int> Update(Feedback? feedback);
    Task<int> Delete(Feedback? feedback);
    
}