using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace Domain.Entities;

public class Colleague : BaseEntity
{
    public string FullNameTj { get; set; }

    
    public string FullNameRu { get; set; }

    
    public string FullNameEn { get; set; }
   
    public string AbouteTj { get; set; }
    public string AbouteRu { get; set; }
    public string AbouteEn { get; set; }
    public string SummaryTj { get; set; }
    public string SummaryRu { get; set; }
    public string SummaryEn { get; set; }
  
    public string Role { get; set; }

    public string ImagePath { get; set; }
    [NotMapped]
    public IFormFile ImageFile { get; set; }
    public List<string> Icons { get; set; }
    
     public List<Course> Courses { get; set; }
}