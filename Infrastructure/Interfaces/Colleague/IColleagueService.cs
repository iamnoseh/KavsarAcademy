using Domain.Dtos.Colleague;
using Domain.Responses;

namespace Infrastructure.Interfaces;

public interface IColleagueService
{
    Task<Response<List<GetColleagueWhitKnowingIcons>>> GetColleaguesWithKnowingIcons(string language = "En");
    Task<Response<GetColleagueWhitKnowingIcons>> GetColleagueWithKnowingIcon(int id,string language = "En");
    Task<Response<GetColleague>> GetColleagueById(int id,string language = "En");
    Task<Response<string>> CreateColleague(CreateColleague request);
    Task<Response<string>> EditColleague(EditColleague request);
    Task<Response<string>> DeleteColleague(int id);
}