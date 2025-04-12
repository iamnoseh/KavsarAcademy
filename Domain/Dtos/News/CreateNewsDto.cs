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
    public string TitleEn { get; set; } = String.Empty;

    public string SummaryTj { get; set; } = String.Empty;
    public string SummaryRu { get; set; } = String.Empty;
    public string SummaryEn { get; set; } = String.Empty;

    [Required]
    public string ContentTj { get; set; } = String.Empty;

    [Required]
    public string ContentEn { get; set; } = String.Empty;

    [Required]
    public string ContentRu { get; set; } = String.Empty;

    [Required, MaxLength(50)]
    public string Category { get; set; } = String.Empty; // Добавляем Category

    [Required, MaxLength(50)]
    public string Author { get; set; } = String.Empty; // Добавляем Author

    public IFormFile Media { get; set; }
}