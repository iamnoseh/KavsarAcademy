using System.Net;
using AutoMapper;
using Domain.Dtos.Request;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;

namespace Infrastructure.Services
{
    public class RequestService : IRequestService
    {
        private readonly IRequestRepository _requestRepository;
        private readonly IMapper _mapper;

        public RequestService(IRequestRepository requestRepository, IMapper mapper)
        {
            _requestRepository = requestRepository;
            _mapper = mapper;
        }

        public async Task<Response<List<GetRequestDto>>> GetAllRequestsAsync()
        {
            var requests = await _requestRepository.GetAll();
            if (!requests.Any())
                return new Response<List<GetRequestDto>>(HttpStatusCode.NotFound, "No requests found");

            var res = _mapper.Map<List<GetRequestDto>>(requests);
            return new Response<List<GetRequestDto>>(res);
        }

        public async Task<Response<GetRequestDto>> GetRequestByIdAsync(int id)
        {
            var request = await _requestRepository.GetRequest(id);
            if (request == null)
                return new Response<GetRequestDto>(HttpStatusCode.NotFound, "No request found");

            var res = _mapper.Map<GetRequestDto>(request);
            return new Response<GetRequestDto>(res);
        }

        public async Task<Response<string>> CreateRequestAsync(CreateRequestDto request)
        {
            // Мап кардани DTO ба Request тавассути AutoMapper
            var newRequest = _mapper.Map<Request>(request);
            newRequest.CreatedAt = DateTime.UtcNow;
            newRequest.UpdatedAt = DateTime.UtcNow;
            newRequest.IsDeleted = false;
            newRequest.DeletedAt = null;

            var res = await _requestRepository.CreateRequest(newRequest);
            return res > 0 
                ? new Response<string>(HttpStatusCode.Created, "Request Created")
                : new Response<string>(HttpStatusCode.BadRequest, "Failed to create request");
        }

        public async Task<Response<string>> UpdateRequestAsync(UpdateRequestDto request)
        {
            var oldRequest = await _requestRepository.GetRequest(request.Id);
            if (oldRequest == null)
                return new Response<string>(HttpStatusCode.NotFound, "No request found");

            _mapper.Map(request, oldRequest);
            oldRequest.UpdatedAt = DateTime.UtcNow;

            var res = await _requestRepository.UpdateRequest(oldRequest);
            return res > 0
                ? new Response<string>(HttpStatusCode.OK, "Request Updated")
                : new Response<string>(HttpStatusCode.BadRequest, "Failed to update request");
        }

        public async Task<Response<string>> DeleteRequestAsync(int id)
        {
            var oldRequest = await _requestRepository.GetRequest(id);
            if (oldRequest == null)
                return new Response<string>(HttpStatusCode.NotFound, "No request found");

            var res = await _requestRepository.DeleteRequest(oldRequest);
            return res > 0 
                ? new Response<string>(HttpStatusCode.OK, "Request Deleted")
                : new Response<string>(HttpStatusCode.BadRequest, "Failed to delete request");
        }

        public async Task<Response<string>> ApproveRequestAsync(int id)
        {
            var approvedRequest = await _requestRepository.GetRequest(id);
            if (approvedRequest == null)
                return new Response<string>(HttpStatusCode.NotFound, "No request found");

            var res = await _requestRepository.ApprovedRequest(approvedRequest);
            return res > 0 
                ? new Response<string>(HttpStatusCode.OK, "Request Approved")
                : new Response<string>(HttpStatusCode.BadRequest, "Failed to approve request");
        }

        public async Task<Response<List<GetRequestDto>>> GetAllApprovedRequestsAsync()
        {
            var approvedRequests = await _requestRepository.GetApprovedRequests();
            if (!approvedRequests.Any())
                return new Response<List<GetRequestDto>>(HttpStatusCode.NotFound, "No approved requests found");

            var res = _mapper.Map<List<GetRequestDto>>(approvedRequests);
            return new Response<List<GetRequestDto>>(res);
        }

        public async Task<Response<GetRequestDto>> GetApprovedRequestsAsync(int id)
        {
            var result = await _requestRepository.GetApprovedRequest(id);
            if (result == null)
                return new Response<GetRequestDto>(HttpStatusCode.NotFound, "No approved request found");

            var res = _mapper.Map<GetRequestDto>(result);
            return new Response<GetRequestDto>(res);
        }
    }
}
