using Infrastructure.Data;
using Infrastructure.Interfaces;
using Infrastructure.Interfaces.LIke;
using Infrastructure.Repositories;
using Infrastructure.Seed;
using Infrastructure.Services;
using Infrastructure.Services.Memory;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExtensionMethods.Register;

public static class Register
{
     public static void AddRegisterService(this IServiceCollection services, IConfiguration configuration)
    {
       services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

       services.AddScoped<IBannerRepository, BannerRepository>();
       services.AddScoped<IRequestRepository, RequestRepository>();
       services.AddScoped<IRequestService, RequestService>();

       services.AddScoped<IChooseUsRepository, ChooseUsRepository>();
// services.AddScoped<IChooseUsService, ChooseUsService>();
       services.AddScoped<SeedData>();


       services.AddScoped<IFeedbackService, FeedbackService>();
       services.AddScoped<IFeedbackRepository, FeedbackRepository>();




       services.AddScoped<IBranchService, BranchService>();
       services.AddScoped<IBranchRepository, BranchRepository>();

       services.AddScoped<ICommentRepository, CommentRepository>();
       services.AddScoped<ICommentService, CommentService>();

//RedisCache
       services.AddScoped<IRedisMemoryCache, RedisMemoryCache>();
       services.AddStackExchangeRedisCache(options =>
       {
           options.Configuration = configuration.GetConnectionString("RedisCache"); 
           options.InstanceName = "Kavsar_"; 
       });
       
       services.AddScoped<ILikeRepository, LikeRepository>();
       services.AddScoped<ILikeService, LikeService>();


       services.AddScoped<INewsRepository, NewsRepository>();

    }
}