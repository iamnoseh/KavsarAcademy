namespace Domain.Dtos.Material;

public class GetMaterialDto
{
    public int Id { get; set; }
    public string? Title { get; set; } // Локализованный заголовок
    public string? Description { get; set; } // Локализованное описание
    public int CourseId { get; set; }
} 