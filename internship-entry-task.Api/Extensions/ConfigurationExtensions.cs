using Microsoft.OpenApi.Models;

namespace internship_entry_task.Api.Extensions;

/// <summary>
/// Расширения для конфигурации служб в приложении.
/// </summary>
/// <remarks>
/// Этот класс содержит методы для добавления аутентификации JWT и настройки Swagger с поддержкой авторизации.
/// </remarks>
public static class ConfigurationExtensions
{
    /// <summary>
    /// Добавляет поддержку Swagger с авторизацией в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Tic Tac Toe",
                Description = "Api игры крестики-нолики",
                Version = "1.1.1.1"
            });
        });
    }
}