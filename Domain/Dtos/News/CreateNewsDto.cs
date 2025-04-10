using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class CreateNewsDto
{
    [Required, MaxLength(100)]
    public string TitleTj { get; set; } = String.Empty;
    [Required, MaxLength(100)]
    public string TitleRu { get; set; } = String.Empty;
    [Required, MaxLength(100)]
    public string TitleEn { get; set; }  = String.Empty;
    public string SummaryTj { get; set; } = String.Empty;
    public string SummaryRu { get; set; } = String.Empty;
    public string SummaryEn { get; set; } = String.Empty;
    [Required]
    public string ContentTj { get; set; } = String.Empty;
    [Required]
    public string ContentEn { get; set; } = String.Empty;
    [Required]
    public string ContentRu { get; set; } = String.Empty;
    
    public IFormFile Media { get; set; } 
    
    public int UserId { get; set; }
}