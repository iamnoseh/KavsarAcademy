using Domain.Dtos.Request;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IRequestService
{
    Task<Response<List<GetRequestDto>>> GetAllRequestsAsync();
    Task<Response<GetRequestDto>> GetRequestByIdAsync(int id);
    Task<Response<string>> CreateRequestAsync(CreateRequestDto request);
    Task<Response<string>> UpdateRequestAsync(UpdateRequestDto request);
    Task<Response<string>> DeleteRequestAsync(int id);
    Task<Response<string>> ApproveRequestAsync(int id);
    Task<Response<List<GetRequestDto>>> GetAllApprovedRequestsAsync();
    Task<Response<GetRequestDto>> GetApprovedRequestsAsync(int id);
}