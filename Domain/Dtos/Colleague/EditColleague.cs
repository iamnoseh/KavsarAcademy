using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.Colleague;

public class EditColleague
{
    public int Id { get; set; }
    public string FullNameTj { get; set; }
    public string FullNameRu { get; set; }
    public string FullNameEn { get; set; }
    

    public string? RoleTj { get; set; }
    public string SummaryTj { get; set; }
    public string SummaryRu { get; set; }
    public string SummaryEn { get; set; }
    
    public string AbouteTj { get; set; }
    public string AbouteRu { get; set; }
    public string AbouteEn { get; set; }
    
    public IFormFile? ProfileImage { get; set; }
    
}