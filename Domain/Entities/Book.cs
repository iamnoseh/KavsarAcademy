using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Book : BaseEntity
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public string UploadPath { get; set; }
    [NotMapped]
    public IFormFile BookPdf { get; set; }
}