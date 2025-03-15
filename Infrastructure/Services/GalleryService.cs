using System.Net;
using Domain.Dtos;
using Domain.Dtos.Gallery;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.Gallery;
using Infrastructure.Services.Memory;

namespace Infrastructure.Services;

public class GalleryService (IGalleryRepository repository,
    IRedisMemoryCache memoryCache,string uploadPath) : IGalleryService
{
    private const string Key = "gallery";
    private const long MaxFileSize = 250 * 1024 * 1024; // 250Mb 
    public async Task<Response<List<GetMediaDto>>> GetMediasAsync()
    {
        var res =  await memoryCache.GetDataAsync<List<GetMediaDto>>(Key);
        if (res == null)
        {
            var medias = await repository.GetAll();
            res = medias.Select(x => new GetMediaDto
            {
                MediaUrl = x.MediaUrl,
            }).ToList();
            await memoryCache.SetDataAsync(Key, res, 3600);
        }

        return new Response<List<GetMediaDto>>(res);
    }

    public async Task<Response<GetMediaDto>> GetMediaAsync(int id)
    {
        var media = await repository.GetById(id);
        if (media == null)
            return new Response<GetMediaDto>(HttpStatusCode.NotFound, "Media not found");
        var res = new GetMediaDto()
        {
            MediaUrl = media.MediaUrl,
        };
        return new Response<GetMediaDto>(res);
    }

    public async Task<Response<string>> CreateMediaAsync(CreateMediaDto createMediaDto)
    {
        if (createMediaDto.MediaFile != null && createMediaDto.MediaFile.Length == 0)
            return new Response<string>(HttpStatusCode.BadRequest, "Media file is required");

        if (createMediaDto.MediaFile != null && createMediaDto.MediaFile.Length > MaxFileSize)
            return new Response<string>(HttpStatusCode.BadRequest, "Media file size must be less than 250Mb");

        if (createMediaDto.MediaFile != null)
        {
            var fileExtension = Path.GetExtension(createMediaDto.MediaFile.FileName).ToLower();
        
            var uploadsFolder = Path.Combine(uploadPath, "uploads", "Gallery");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await createMediaDto.MediaFile.CopyToAsync(fileStream);
            }

            var media = new Gallery()
            {
                CreatedAt = DateTime.UtcNow,
                MediaUrl = $"/uploads/Gallery/{uniqueFileName}",
            };
            var res = await repository.CreateMedia(media);
            if (res <= 0) return new Response<string>(HttpStatusCode.BadGateway, "Something went wrong");
        }

        await memoryCache.RemoveDataAsync(Key);
        return new Response<string>(HttpStatusCode.Created, "Media added");
    }

    public async Task<Response<string>> EditMediaAsync(EditMediaDto editMediaDto)
    {
        var media = await repository.GetById(editMediaDto.Id);
        if (media == null)
            return new Response<string>(HttpStatusCode.NotFound, "Media not found");
        media.Id = editMediaDto.Id;
        media.UpdatedAt = DateTime.UtcNow;
        {
            if (editMediaDto.MediaFile.Length > MaxFileSize)
                return new Response<string>(HttpStatusCode.BadRequest,
                    "Image file size must be less than 250MB");

            var fileExtension = Path.GetExtension(editMediaDto.MediaFile.FileName).ToLower();

            var uploadsFolder = Path.Combine(uploadPath, "uploads", "Gallery");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);
            await using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await editMediaDto.MediaFile.CopyToAsync(fileStream);
            }

            media.MediaUrl = $"/uploads/news/{uniqueFileName}";
        }
        var res = await repository.EditMedia(media);
        if (res <= 0) return new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
        await memoryCache.RemoveDataAsync(Key);
        return new Response<string>(HttpStatusCode.OK, "Media edited");
    }

    public async Task<Response<string>> DeleteMediaAsync(int id)
    {
        var media = await repository.GetById(id);
        if (media == null)
            return new Response<string>(HttpStatusCode.NotFound, "Media not found");
        await memoryCache.RemoveDataAsync(Key);
        var res = await repository.DeleteMedia(media);
        return res > 0 
            ? new Response<string>(HttpStatusCode.OK, "Media deleted")
            : new Response<string>(HttpStatusCode.NotFound, "Media not found");
    }
}