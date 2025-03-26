namespace Domain.Entities;

public class Material
{
    public int Id { get; set; }
    
    // Заголовки на трёх языках
    public string? TitleTj { get; set; }
    public string? TitleRu { get; set; }
    public string? TitleEn { get; set; }
    
    // Описания на трёх языках
    public string? DescriptionTj { get; set; }
    public string? DescriptionRu { get; set; }
    public string? DescriptionEn { get; set; }
    
    // Связь с курсом
    public int CourseId { get; set; }
    public Course Course { get; set; }
}
