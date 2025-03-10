using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class CreateMediaDto
{
    public IFormFile MediaFile { get; set; }
}