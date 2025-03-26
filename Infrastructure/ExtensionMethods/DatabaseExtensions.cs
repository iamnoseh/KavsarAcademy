using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.ExtensionMethods;

public static class DatabaseExtensions
{
    /// <summary>
    /// Добавляет контекст базы данных в приложение
    /// </summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        // Получаем строку подключения из конфигурации
        var connectionString = configuration.GetConnectionString("DefaultConnection");
        
        // Регистрируем контекст базы данных
        services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(connectionString)
        );
        
        return services;
    }
} 