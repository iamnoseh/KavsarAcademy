namespace Domain.Dtos.Material;

public class CreateMaterialDto
{
    // Заголовки на трёх языках
    public string? TitleTj { get; set; }
    public string? TitleRu { get; set; }
    public string? TitleEn { get; set; }
    
    // Описания на трёх языках
    public string? DescriptionTj { get; set; }
    public string? DescriptionRu { get; set; }
    public string? DescriptionEn { get; set; }
    
    public int CourseId { get; set; }
} 