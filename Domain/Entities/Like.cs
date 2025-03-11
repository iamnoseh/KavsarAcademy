using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Like
{
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User? User { get; set; }

    public int? NewsId { get; set; }
    public News? News { get; set; }
}