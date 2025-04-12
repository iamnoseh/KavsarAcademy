using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class UpdateNewsDto
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string TitleTj { get; set; }

    [Required, MaxLength(100)]
    public string TitleRu { get; set; }

    [Required, MaxLength(100)]
    public string TitleEn { get; set; }

    public string SummaryTj { get; set; } = String.Empty;
    public string SummaryRu { get; set; } = String.Empty;
    public string SummaryEn { get; set; } = String.Empty;

    [Required]
    public string ContentTj { get; set; }

    [Required]
    public string ContentEn { get; set; }

    [Required]
    public string ContentRu { get; set; }

    [Required, MaxLength(50)]
    public string Category { get; set; } // Добавляем Category

    [Required, MaxLength(50)]
    public string Author { get; set; } // Добавляем Author

    public IFormFile? Media { get; set; } // Делаем необязательным
}