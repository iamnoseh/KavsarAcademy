using System;

namespace Domain.Dtos;

public class GetNewsDto
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Summary { get; set; }
    public string? Content { get; set; }
    public string? Category { get; set; } // Добавляем Category
    public string? Author { get; set; } // Добавляем Author

    
    public string? Image => MediaUrl; // Добавляем Image как псевдоним для MediaUrl
    public string? MediaUrl { get; set; }
    public int LikeCount { get; set; }
    public DateTime CreatedAt { get; set; }
}