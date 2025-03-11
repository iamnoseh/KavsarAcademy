using System.ComponentModel.DataAnnotations;

namespace Domain.Dtos;

public class CreateLikeDto
{
    [Required]
    public int UserId { get; set; }
    public int? NewsId { get; set; }
}

public class DeleteLikeDto
{
    public int Id { get; set; }
    public int? UserId { get; set; }
    public int? NewsId { get; set; }
}