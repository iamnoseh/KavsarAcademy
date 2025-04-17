namespace Domain.Dtos.Colleague;

public class GetColleagueWhitKnowingIcons
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string Aboute { get; set; }
    public string Summary { get; set; }

    public string ProfileImagePath { get; set; }
    public string Role { get; set; }
    public List<string> KnowingIcons { get; set; }
}