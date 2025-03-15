using Domain.Dtos;
using Domain.Dtos.Gallery;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IGalleryService
{
    Task<Response<List<GetMediaDto>>> GetMediasAsync();
    Task<Response<GetMediaDto>> GetMediaAsync(int id);
    Task<Response<string>> CreateMediaAsync(CreateMediaDto createMediaDto);
    Task<Response<string>> EditMediaAsync(EditMediaDto editMediaDto);
    Task<Response<string>> DeleteMediaAsync(int id);
}