using Microsoft.AspNetCore.Http;

namespace Domain.Dtos.VideoReview;

public class CreateVideoReview
{
    public string ReviewerNameTj { get; set; } = string.Empty;
    public string ReviewerNameRu { get; set; } = string.Empty;
    public string ReviewerNameEn { get; set; } = string.Empty;
    public IFormFile? VideoReviewFile { get; set; }
}