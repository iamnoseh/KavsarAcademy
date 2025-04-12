using System.Net;
using Domain.Dtos;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Services.Memory;
using Ganss.XSS; // Для санитизации HTML

namespace Infrastructure.Services;

public class NewsService : INewsService
{
    private readonly INewsRepository repository;
    private readonly IRedisMemoryCache memoryCache;
    private readonly string uploadPath;
    private readonly HtmlSanitizer sanitizer;
    private readonly string[] _allowedExtensions = { ".jpg", ".jpeg", ".png", ".gif" };
    private const long MaxFileSize = 100 * 1024 * 1024; // 100MB
    private const string Key = "news";

    public NewsService(INewsRepository repository, IRedisMemoryCache memoryCache, string uploadPath)
    {
        this.repository = repository;
        this.memoryCache = memoryCache;
        this.uploadPath = uploadPath;
        this.sanitizer = new HtmlSanitizer();
        sanitizer.AllowedTags.Add("p");
        sanitizer.AllowedTags.Add("ul");
        sanitizer.AllowedTags.Add("li");
        sanitizer.AllowedTags.Add("b");
        sanitizer.AllowedTags.Add("i");
        sanitizer.AllowedTags.Add("strong");
    }

    public async Task<Response<List<GetNewsDto>>> GetNewsAsync(string language = "En")
    {
        var newsType = typeof(News);
        var news = await memoryCache.GetDataAsync<List<GetNewsDto>>(Key);
        if (news == null)
        {
            var newsData = await repository.GetAllNews();
            news = newsData.Select(x => new GetNewsDto
            {
                Id = x.Id,
                Title = newsType.GetProperty("Title" + language)?.GetValue(x)?.ToString(),
                Summary = newsType.GetProperty("Summary" + language)?.GetValue(x)?.ToString(),
                Content = newsType.GetProperty("Content" + language)?.GetValue(x)?.ToString(),
                CreatedAt = x.CreatedAt,
                LikeCount = x.Likes.Count,
                MediaUrl = x.MediaUrl,
                Category = x.Category,
                Author = x.Author,
            }).ToList();
            await memoryCache.SetDataAsync(Key, news, 10);
        }

        return new Response<List<GetNewsDto>>(news);
    }

    public async Task<Response<GetNewsDto>> GetNewsByIdAsync(int id, string language = "En")
    {
        var newsType = typeof(News);
        var news = await repository.GetNewsById(id);
        if (news == null)
            return new Response<GetNewsDto>(HttpStatusCode.NotFound, "News not found");

        var dto = new GetNewsDto
        {
            Id = news.Id,
            Title = newsType.GetProperty("Title" + language)?.GetValue(news)?.ToString(),
            Summary = newsType.GetProperty("Summary" + language)?.GetValue(news)?.ToString(),
            Content = newsType.GetProperty("Content" + language)?.GetValue(news)?.ToString(),
            CreatedAt = news.CreatedAt,
            LikeCount = news.Likes.Count,
            
            MediaUrl = news.MediaUrl,
            Category = news.Category,
            Author = news.Author
        };
        return new Response<GetNewsDto>(dto);
    }

    public async Task<Response<string>> CreateNewsAsync(CreateNewsDto request)
    {
        if (request.Media == null || request.Media.Length == 0)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file is required");

        if (request.Media.Length > MaxFileSize)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 100MB");

        var fileExtension = Path.GetExtension(request.Media.FileName).ToLower();
        if (!_allowedExtensions.Contains(fileExtension))
            return new Response<string>(HttpStatusCode.BadRequest, "Invalid file format. Allowed formats: .jpg, .jpeg, .png, .gif");

        // Санитизируем content для каждого языка
        var sanitizedContentTj = sanitizer.Sanitize(request.ContentTj);
        var sanitizedContentRu = sanitizer.Sanitize(request.ContentRu);
        var sanitizedContentEn = sanitizer.Sanitize(request.ContentEn);

        if (string.IsNullOrWhiteSpace(sanitizedContentTj) || string.IsNullOrWhiteSpace(sanitizedContentRu) || string.IsNullOrWhiteSpace(sanitizedContentEn))
            return new Response<string>(HttpStatusCode.BadRequest, "Content cannot be empty after sanitization");

        var uploadsFolder = Path.Combine(uploadPath, "uploads", "news");
        if (!Directory.Exists(uploadsFolder))
            Directory.CreateDirectory(uploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsFolder, uniqueFileName);

        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.Media.CopyToAsync(fileStream);
        }

        var news = new News
        {
            
            TitleTj = request.TitleTj,
            TitleRu = request.TitleRu,
            TitleEn = request.TitleEn,
            ContentTj = sanitizedContentTj,
            ContentRu = sanitizedContentRu,
            ContentEn = sanitizedContentEn,
            SummaryTj = request.SummaryTj,
            SummaryRu = request.SummaryRu,
            SummaryEn = request.SummaryEn,
            CreatedAt = DateTime.UtcNow,
            MediaUrl = $"/uploads/news/{uniqueFileName}",
            Category = request.Category,
            Author = request.Author
        };

        int res = await repository.CreateNews(news);
        if (res > 0)
        {
            await memoryCache.RemoveDataAsync(Key);
            return new Response<string>(HttpStatusCode.Created, "News created");
        }
        return new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> UpdateNewsAsync(UpdateNewsDto request)
    {
        var oldNews = await repository.GetNewsById(request.Id);
        if (oldNews == null)
            return new Response<string>(HttpStatusCode.NotFound, "News not found");

        // Санитизируем content для каждого языка
        var sanitizedContentTj = sanitizer.Sanitize(request.ContentTj);
        var sanitizedContentRu = sanitizer.Sanitize(request.ContentRu);
        var sanitizedContentEn = sanitizer.Sanitize(request.ContentEn);

        if (string.IsNullOrWhiteSpace(sanitizedContentTj) || string.IsNullOrWhiteSpace(sanitizedContentRu) || string.IsNullOrWhiteSpace(sanitizedContentEn))
            return new Response<string>(HttpStatusCode.BadRequest, "Content cannot be empty after sanitization");

        oldNews.TitleTj = request.TitleTj;
        oldNews.TitleRu = request.TitleRu;
        oldNews.TitleEn = request.TitleEn;
        oldNews.ContentTj = sanitizedContentTj;
        oldNews.ContentRu = sanitizedContentRu;
        oldNews.ContentEn = sanitizedContentEn;
        oldNews.SummaryTj = request.SummaryTj;
        oldNews.SummaryRu = request.SummaryRu;
        oldNews.SummaryEn = request.SummaryEn;
        oldNews.Category = request.Category;
        oldNews.Author = request.Author;
        oldNews.UpdatedAt = DateTime.UtcNow;

        if (request.Media != null && request.Media.Length > 0)
        {
            if (request.Media.Length > MaxFileSize)
                return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 100MB");

            var fileExtension = Path.GetExtension(request.Media.FileName).ToLower();
            if (!_allowedExtensions.Contains(fileExtension))
                return new Response<string>(HttpStatusCode.BadRequest, "Invalid file format. Allowed formats: .jpg, .jpeg, .png, .gif");

            var uploadsFolder = Path.Combine(uploadPath, "uploads", "news");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await request.Media.CopyToAsync(fileStream);
            }

            // Удаляем старый файл
            if (!string.IsNullOrEmpty(oldNews.MediaUrl))
            {
                var oldFilePath = Path.Combine(uploadPath, oldNews.MediaUrl.TrimStart('/'));
                if (File.Exists(oldFilePath))
                    File.Delete(oldFilePath);
            }

            oldNews.MediaUrl = $"/uploads/news/{uniqueFileName}";
        }

        var res = await repository.UpdateNews(oldNews);
        if (res > 0)
        {
            await memoryCache.RemoveDataAsync(Key);
            return new Response<string>(HttpStatusCode.NoContent, "News updated");
        }
        return new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

    public async Task<Response<string>> DeleteNewsAsync(int id)
    {
        var deletedNews = await repository.GetNewsById(id);
        if (deletedNews == null)
            return new Response<string>(HttpStatusCode.NotFound, "News not found");

        // Удаляем связанный файл
        if (!string.IsNullOrEmpty(deletedNews.MediaUrl))
        {
            var filePath = Path.Combine(uploadPath, deletedNews.MediaUrl.TrimStart('/'));
            if (File.Exists(filePath))
                File.Delete(filePath);
        }

        int res = await repository.DeleteNews(deletedNews);
        if (res > 0)
        {
            await memoryCache.RemoveDataAsync(Key);
            return new Response<string>(HttpStatusCode.NoContent, "News deleted");
        }
        return new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }
}