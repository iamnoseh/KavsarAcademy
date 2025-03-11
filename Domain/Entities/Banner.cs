using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
namespace Domain.Entities;

public class Banner : BaseEntity
{
    public string TitleTj { get; set; } = string.Empty;
    public string TitleRu { get; set; } 
    public string TitleEn { get; set; } 
    
    public string DescriptionTj { get; set; }
    public string DescriptionRu { get; set; }
    public string DescriptionEn { get; set; }
    
    public string ImagePath { get; set; }
    
    [NotMapped]
    public IFormFile Image { get; set; }
}