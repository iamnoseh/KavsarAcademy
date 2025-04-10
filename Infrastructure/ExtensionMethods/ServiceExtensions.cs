
using Infrastructure.Interfaces;

using Infrastructure.Repositories;
using Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExtensionMethods;

public static class ServiceExtensions
{
    /// <summary>
    /// Регистрирует все сервисы приложения
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string uploadPath)
    {
        // Регистрация репозиториев и сервисов
        services.AddCourseServices(uploadPath);
        services.AddMaterialServices();
        // services.AddStudyInCourseServices(uploadPath);
        services.AddColleagueServices(uploadPath);
        services.AddGalleryServices(uploadPath);
        services.AddNewsServices();
        services.AddBranchServices();
        services.AddUserServices();
        
        return services;
    }
    
    /// <summary>
    /// Регистрирует сервисы для работы с курсами
    /// </summary>
    public static IServiceCollection AddCourseServices(this IServiceCollection services, string uploadPath)
    {
        // Регистрация репозитория и сервиса курсов
        services.AddScoped<ICourseRepository, CourseRepository>();
        
        // Передача uploadPath для сохранения изображений
        services.AddScoped<ICourseService>(provider => 
            new CourseService(
                provider.GetRequiredService<ICourseRepository>(),
                uploadPath
            )
        );
        
        return services;
    }
    
    /// <summary>
    /// Регистрирует сервисы для работы с материалами
    /// </summary>
    public static IServiceCollection AddMaterialServices(this IServiceCollection services)
    {
        // Регистрация репозитория и сервиса материалов
        // services.AddScoped<IMaterialRepository, MaterialRepository>();
        // services.AddScoped<IMaterialService, MaterialService>();
        
        return services;
    }
    
    /// <summary>
    /// Регистрирует сервисы для работы с StudyInCourse
    /// </summary>
   
    
    /// <summary>
    /// Регистрирует сервисы для работы с преподавателями (Colleague)
    /// </summary>
    public static IServiceCollection AddColleagueServices(this IServiceCollection services, string uploadPath)
    {
        // Регистрация репозитория
        services.AddScoped<IColleagueRepository, ColleagueRepository>();
        
        // Регистрация сервиса с передачей пути для загрузки файлов
        services.AddScoped<IColleagueService>(provider => 
            new ColleagueService(
                provider.GetRequiredService<IColleagueRepository>(),
                uploadPath
            )
        );
        
        return services;
    }
    
    /// <summary>
    /// Регистрирует сервисы для работы с галереей (Gallery)
    /// </summary>
    public static IServiceCollection AddGalleryServices(this IServiceCollection services, string uploadPath)
    {
        // Здесь добавьте регистрацию сервисов галереи
        // Пример:
        // services.AddScoped<IGalleryRepository, GalleryRepository>();
        // services.AddScoped<IGalleryService>(provider => 
        //     new GalleryService(
        //         provider.GetRequiredService<IGalleryRepository>(),
        //         uploadPath
        //     )
        // );
        
        return services;
    }
    
    /// <summary>
    /// Регистрирует сервисы для работы с новостями (News)
    /// </summary>
    public static IServiceCollection AddNewsServices(this IServiceCollection services)
    {
        // Здесь добавьте регистрацию сервисов новостей
        // Пример:
        // services.AddScoped<INewsRepository, NewsRepository>();
        // services.AddScoped<INewsService, NewsService>();
        
        return services;
    }
    
    /// <summary>
    /// Регистрирует сервисы для работы с филиалами (Branch)
    /// </summary>
    public static IServiceCollection AddBranchServices(this IServiceCollection services)
    {
        // Здесь добавьте регистрацию сервисов филиалов
        // Пример:
        // services.AddScoped<IBranchRepository, BranchRepository>();
        // services.AddScoped<IBranchService, BranchService>();
        
        return services;
    }
    
    /// <summary>
    /// Регистрирует сервисы для работы с пользователями
    /// </summary>
    public static IServiceCollection AddUserServices(this IServiceCollection services)
    {
        // Здесь добавьте регистрацию сервисов пользователей
        // Пример:
        // services.AddScoped<IUserRepository, UserRepository>();
        // services.AddScoped<IUserService, UserService>();
        
        return services;
    }
} 