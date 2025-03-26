using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Infrastructure.ExtensionMethods;

public static class ConfigureAppExtensions
{
    /// <summary>
    /// Настраивает middleware для приложения
    /// </summary>
    public static WebApplication ConfigureApplication(this WebApplication app)
    {
        // Настройка Swagger в зависимости от окружения
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        // Перенаправление HTTP запросов на HTTPS
        app.UseHttpsRedirection();
        
        // Настройка статических файлов для доступа к загруженным файлам
        app.UseStaticFiles();
        
        // Настройка аутентификации и авторизации
        app.UseAuthentication();
        app.UseAuthorization();
        
        // Настройка эндпоинтов
        app.MapControllers();
        
        return app;
    }
} 