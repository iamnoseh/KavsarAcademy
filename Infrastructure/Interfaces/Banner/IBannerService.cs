using System.Threading.Tasks;
using Domain.Dtos.BannerDto;
using Domain.Responses;

namespace Infrastructure.Interfaces.IServices;

public interface IBannerService
{
    Task<Response<List<GetBannerDto>>> GetAllBanners(string language = "en");
    Task<Response<GetBannerDto>> GetBannerById(int id, string language = "en");
    Task<Response<string>> CreateBanner(CreateBannerDto dto);
    Task<Response<string>> UpdateBanner(UpdateBannerDto dto);
    Task<Response<string>> DeleteBanner(int id);
}

