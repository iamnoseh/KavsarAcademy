using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;

namespace Domain.Entities;

public class Gallery : BaseEntity
{
    public string MediaUrl { get; set; }
    [NotMapped]
    public IFormFile MediaFile { get; set; }
}