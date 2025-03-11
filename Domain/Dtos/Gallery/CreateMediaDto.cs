using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.Gallery;

public class CreateMediaDto
{
    public IFormFile? MediaFile { get; set; }
}