using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace Domain.Entities;

public class Colleague : BaseEntity
{
    public string FirstNameTj { get; set; }
    public string LastNameTj { get; set; }
    
    public string FirstNameRu { get; set; }
    public string LastNameRu { get; set; }
    
    public string FirstNameEn { get; set; }
    public string LastNameEn { get; set; }
    
    public string AbouteTj { get; set; }
    public string AbouteRu { get; set; }
    public string AbouteEn { get; set; }
  
    public string ImagePath { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public List<string> Icons { get; set; }
}