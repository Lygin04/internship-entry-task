
using internship_entry_task.Application.Services;
using internship_entry_task.Application.Services.Interfaces;

namespace internship_entry_task.Api.Extensions;

/// <summary>
/// Расширения для настройки сервисов приложения.
/// </summary>
/// <remarks>
/// Этот класс содержит методы для добавления сервисов приложения в контейнер зависимостей.
/// </remarks>
public static class ApplicationHostExtensions
{
    /// <summary>
    /// Добавляет сервисы приложения в коллекцию сервисов.
    /// </summary>
    /// <param name="services">Коллекция сервисов.</param>
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IGameService, GameService>();
    }
}