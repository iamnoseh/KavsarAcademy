using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User : IdentityUser<int>
{
    [Required]
    [StringLength(50, MinimumLength = 3, ErrorMessage = "Firstname must be between 3 and 50 characters")]
    public string FirstName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "Lastname must be between 3 and 50 characters")]
    public string? LastName { get; set; }
    public int Age { get; set; }
    [Required]
    public string Address { get; set; }
    public bool IsActive { get; set; }
    public DateTime RegistrationDate { get; set; } 
    [NotMapped]
    public IFormFile? ProfileImage { get; set; }
    public string? ProfileImagePath { get; set; }
    
    public List<Like> Likes { get; set; } = new();
    public List<Comment> Comments { get; set; } = new();
    public List<News> News { get; set; } = new();
    public List<Feedback> Feedbacks { get; set; } = new();
}