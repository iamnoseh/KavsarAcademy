namespace Infrastructure.Interfaces.Gallery;

public interface IGalleryRepository
{
    Task<List<Domain.Entities.Gallery>> GetAll();
    Task<Domain.Entities.Gallery?> GetById(int id);
    Task<int> CreateMedia(Domain.Entities.Gallery request);
    Task<int> EditMedia(Domain.Entities.Gallery request);
    Task<int> DeleteMedia(Domain.Entities.Gallery request);
}