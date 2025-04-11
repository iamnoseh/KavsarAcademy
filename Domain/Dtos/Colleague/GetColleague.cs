namespace Domain.Dtos.Colleague;

public class GetColleague
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string About { get; set; }
    public string? Role { get; set; }
    public string? ProfileImage { get; set; }
}