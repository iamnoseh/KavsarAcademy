using Microsoft.AspNetCore.Http;

namespace Domain.Dtos;

public class EditMediaDto
{
    public int Id { get; set; }
    public IFormFile MediaFile { get; set; }
}