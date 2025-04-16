using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.Colleague;

public class EditColleague
{
    public int Id { get; set; }
    public string FirstNameTj { get; set; }
    public string FirstNameRu { get; set; }
    public string FirstNameEn { get; set; }
    

    public string? RoleTj { get; set; }

    
    public string AbouteTj { get; set; }
    public string AbouteRu { get; set; }
    public string AbouteEn { get; set; }
    
    public IFormFile? ProfileImage { get; set; }
    
}