using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.Colleague;
public class CreateColleague
{
    public string FirstNameTj { get; set; }
    
    public string FirstNameRu { get; set; }
    
    public string FirstNameEn { get; set; }
  
    public string AbouteTj { get; set; }
    public string AbouteRu { get; set; }
    public string AbouteEn { get; set; }
    public string? RoleTj { get; set; }

    public IFormFile ImageFile { get; set; }
    public List<IFormFile> IconFiles { get; set; } // Барои иконҳо
}