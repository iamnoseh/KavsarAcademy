using System.Net;
using System.Reflection;
using Domain.Dtos.Colleague;
using Domain.Entities;
using Domain.Responses;
using Infrastructure.Interfaces;

namespace Infrastructure.Services;

public class ColleagueService(IColleagueRepository repository, 
    string uploadPath) : IColleagueService
{
    private readonly string[] _allowedExtensions = [".jpg", ".jpeg", ".png", ".gif",".svg"];
    private const long MaxFileSize = 10 * 1024 * 1024; // 10MB
    public async Task<Response<List<GetColleagueWhitKnowingIcons>>> GetColleaguesWithKnowingIcons(string language = "En")
    {
        var colleagueType = typeof(Colleague);
        var colleagues = await repository.GetAll();
    
        if (!colleagues.Any())
            return new Response<List<GetColleagueWhitKnowingIcons>>(HttpStatusCode.NotFound, "Colleague not found");

        var dto = colleagues.Select(x => new GetColleagueWhitKnowingIcons
        {
            Id = x.Id,
            FullName = colleagueType.GetProperty("FullName" + language)?.GetValue(x)?.ToString() ?? string.Empty,
            Aboute = colleagueType.GetProperty("Aboute" + language)?.GetValue(x)?.ToString() ?? string.Empty,
            Summary = colleagueType.GetProperty("Summary" + language)?.GetValue(x)?.ToString() ?? string.Empty,
            Role = colleagueType.GetProperty("Role" + language)?.GetValue(x)?.ToString() ?? string.Empty,
            ProfileImagePath = x.ImagePath,
            KnowingIcons = x.Icons.ToList()
        }).ToList();

        return new Response<List<GetColleagueWhitKnowingIcons>>(dto);
    }
    public async Task<Response<GetColleagueWhitKnowingIcons>> GetColleagueWithKnowingIcon(int id, string language = "En")
    {
        var colleagueType = typeof(Colleague);
        var colleague = await repository.GetById(id);
        if (colleague == null)
            return new Response<GetColleagueWhitKnowingIcons>(HttpStatusCode.NotFound, "Colleague not found");

        var dto = new GetColleagueWhitKnowingIcons
        {
            Id = colleague.Id,
            FullName = colleagueType.GetProperty("FullName" + language)?.GetValue(colleague).ToString(),
            Aboute = colleagueType.GetProperty("Aboute" + language)?.GetValue(colleague).ToString(),
            Role = colleagueType.GetProperty("Role" + language)?.GetValue(colleague).ToString(),
            Summary = colleagueType.GetProperty("Summary" + language)?.GetValue(colleague).ToString(),
            ProfileImagePath = colleague.ImagePath,
            KnowingIcons = colleague.Icons.ToList()
        };
        return new Response<GetColleagueWhitKnowingIcons>(dto);
    }

    public async Task<Response<GetColleague>> GetColleagueById(int id, string language = "En")
    {
        var colleagueType = typeof(Colleague);
        var colleague = await repository.GetById(id);
        if (colleague == null)
            return new Response<GetColleague>(HttpStatusCode.NotFound, "Colleague not found");

        var dto = new GetColleague
        {
            Id = colleague.Id,
            FullName = colleagueType.GetProperty("FullName" + language)?.GetValue(colleague).ToString(),
            About = colleagueType.GetProperty("Aboute" + language)?.GetValue(colleague).ToString(),
            Role = colleagueType.GetProperty("Role" + language)?.GetValue(colleague).ToString(),
            Summary = colleagueType.GetProperty("Summary" + language).GetValue(colleague).ToString(), 
            ProfileImage = colleague.ImagePath,
        };
        return new Response<GetColleague>(dto);
    }

    public async Task<Response<string>> CreateColleague(CreateColleague request)
    {
        if (request.ImageFile.Length == 0)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file is required");

        if (request.ImageFile.Length > MaxFileSize)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 10MB");

        var fileExtension = Path.GetExtension(request.ImageFile.FileName).ToLower();
        if (!_allowedExtensions.Contains(fileExtension))
            return new Response<string>(HttpStatusCode.BadRequest, "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");
        
        var colleagueUploadsFolder = Path.Combine(uploadPath, "uploads", "Colleague");
        if (!Directory.Exists(colleagueUploadsFolder))
            Directory.CreateDirectory(colleagueUploadsFolder);

        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(colleagueUploadsFolder, uniqueFileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.ImageFile.CopyToAsync(fileStream);
        }

        var colleague = new Colleague
        {
            FullNameTj = request.FullNameTj,

            FullNameRu = request.FullNameRu,
 
            FullNameEn = request.FullNameEn,
   
            AbouteTj = request.AbouteTj,
            AbouteRu = request.AbouteRu,
            AbouteEn = request.AbouteEn,
            CreatedAt = DateTime.UtcNow,
            SummaryEn = request.SummaryEn,
            SummaryTj = request.SummaryTj,
            SummaryRu = request.SummaryRu,
            RoleTj = request.RoleTj,
            ImagePath = $"/uploads/Colleague/{uniqueFileName}",
            Icons = new List<string>()
        };
        
        if (request.IconFiles.Any())
        {
            var galleryUploadsFolder = Path.Combine(uploadPath, "uploads", "Gallery");
            if (!Directory.Exists(galleryUploadsFolder))
                Directory.CreateDirectory(galleryUploadsFolder);

            foreach (var iconFile in request.IconFiles)
            {
                if (iconFile.Length > MaxFileSize)
                    return new Response<string>(HttpStatusCode.BadRequest, "Icon file size must be less than 10MB");

                var iconExtension = Path.GetExtension(iconFile.FileName).ToLower();
                if (!_allowedExtensions.Contains(iconExtension))
                    return new Response<string>(HttpStatusCode.BadRequest, "Invalid icon format. Allowed formats: .jpg, .jpeg, .png, .gif");

                var iconUniqueFileName = $"{Guid.NewGuid()}{iconExtension}";
                var iconFilePath = Path.Combine(galleryUploadsFolder, iconUniqueFileName);
                await using (var iconStream = new FileStream(iconFilePath, FileMode.Create))
                {
                    await iconFile.CopyToAsync(iconStream);
                }
                string mediaUrl = $"/uploads/Gallery/{iconUniqueFileName}";
                
                colleague.Icons.Add(mediaUrl);
            }
        }

        int res = await repository.Create(colleague);
        return res > 0
            ? new Response<string>(HttpStatusCode.OK, "Colleague Created Successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
    }

  public async Task<Response<string>> EditColleague(EditColleague request)
{
    var colleague = await repository.GetById(request.Id);
    if (colleague == null)
        return new Response<string>(HttpStatusCode.NotFound, "Colleague not found");

    colleague.FullNameTj = request.FullNameTj;

    colleague.FullNameRu = request.FullNameRu;

    colleague.FullNameEn = request.FullNameEn;

    colleague.AbouteTj = request.AbouteTj;
    colleague.AbouteRu = request.AbouteRu;
    colleague.AbouteEn = request.AbouteEn;
    colleague.SummaryEn = request.SummaryEn;
    colleague.SummaryTj = request.SummaryTj;
    colleague.SummaryRu = request.SummaryRu;
    colleague.RoleTj = request.RoleTj;
    
    
    if (request.ProfileImage != null && request.ProfileImage.Length > 0)
    {
        if (request.ProfileImage.Length > MaxFileSize)
            return new Response<string>(HttpStatusCode.BadRequest, "Image file size must be less than 10MB");

        var fileExtension = Path.GetExtension(request.ProfileImage.FileName).ToLower();
        if (!_allowedExtensions.Contains(fileExtension))
            return new Response<string>(HttpStatusCode.BadRequest, "Invalid image format. Allowed formats: .jpg, .jpeg, .png, .gif");

        var colleagueUploadsFolder = Path.Combine(uploadPath, "uploads", "Colleague");
        if (!Directory.Exists(colleagueUploadsFolder))
            Directory.CreateDirectory(colleagueUploadsFolder);
        
        var uniqueFileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(colleagueUploadsFolder, uniqueFileName);
        await using (var fileStream = new FileStream(filePath, FileMode.Create))
        {
            await request.ProfileImage.CopyToAsync(fileStream);
        }
        colleague.ImagePath = $"/uploads/Colleague/{uniqueFileName}";
    }
    
    int res = await repository.Update(colleague);
    return res > 0
        ? new Response<string>(HttpStatusCode.OK, "Colleague updated successfully")
        : new Response<string>(HttpStatusCode.BadRequest, "Something went wrong");
}


    public async Task<Response<string>> DeleteColleague(int id)
    {
        var colleague = await repository.GetById(id);
        if (colleague == null)
            return new Response<string>(HttpStatusCode.NotFound, "Colleague not found");

        var res = await repository.Delete(colleague);
        return res > 0
            ? new Response<string>(HttpStatusCode.OK, "Colleague deleted successfully")
            : new Response<string>(HttpStatusCode.BadRequest, "Failed to delete Colleague");
    }
}
