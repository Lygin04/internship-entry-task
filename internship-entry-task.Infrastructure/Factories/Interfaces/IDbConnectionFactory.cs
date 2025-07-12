using System.Data.Common;

namespace internship_entry_task.Infrastructure.Factories.Interfaces;

/// <summary>
/// Представляет интерфейс фабрики подключений к базе данных.
/// </summary>
public interface IDbConnectionFactory
{
    /// <summary>
    /// Создает асинхронное подключение к базе данных.
    /// </summary>
    Task<DbConnection> CreateAsync();
}