using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class VideoReview : BaseEntity
{
    public int Id { get; set; }
    [Required]
    public string ReviewerNameTj { get; set; } = string.Empty;
    public string ReviewerNameRu { get; set; } = string.Empty;
    public string ReviewerNameEn { get; set; } = string.Empty;
    public string ReviewPath { get; set; } = string.Empty;
    [NotMapped]
    public IFormFile? File { get; set; }
}