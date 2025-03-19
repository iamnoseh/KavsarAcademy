namespace Domain.Dtos.Colleague;

public class GetColleagueWhitKnowingIcons
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Aboute { get; set; }
    public string ProfileImagePath { get; set; }
    public List<string> KnowingIcons { get; set; }
}