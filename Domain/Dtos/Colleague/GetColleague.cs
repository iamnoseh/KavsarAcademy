namespace Domain.Dtos.Colleague;

public class GetColleague
{
    public int Id { get; set; }
    public string FullName { get; set; }
    public string About { get; set; }
    public string Summary { get; set; }

    public string? Role { get; set; }
    public string? ProfileImage { get; set; }
}