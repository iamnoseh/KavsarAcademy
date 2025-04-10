using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class News : BaseEntity
{
    [Required]
    [MaxLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
    public string TitleTj { get; set; }

    [Required]
    [MaxLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
    public string TitleRu { get; set; }

    [Required]
    [MaxLength(150, ErrorMessage = "Title cannot be longer than 150 characters.")]
    public string TitleEn { get; set; }

    [Required]
    [MaxLength(300, ErrorMessage = "Title cannot be longer than 300 characters.")]
    public string SummaryTj { get; set; } = string.Empty;
    [Required]
    [MaxLength(300, ErrorMessage = "Title cannot be longer than 300 characters.")]
    public string SummaryRu { get; set; } = string.Empty;
    [Required]
    [MaxLength(300, ErrorMessage = "Title cannot be longer than 300 characters.")]
    public string SummaryEn { get; set; } = string.Empty;
    [Required]
    public string ContentTj { get; set; }

    [Required] 
    public string ContentEn { get; set; }

    [Required]
    public string ContentRu { get; set; }

    [Required]
    public int UserId { get; set; }

    [ForeignKey("UserId")] 
    public virtual User User { get; set; } = null!; 

    public string? MediaUrl { get; set; }

    [NotMapped]
    public IFormFile? Media { get; set; }

    public List<Comment> Comments { get; set; } = new();
    public List<Like> Likes { get; set; } = new();

    [NotMapped] 
    public int LikeCount => Likes?.Count ?? 0;
}