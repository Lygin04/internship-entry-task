using internship_entry_task.Domain.Entities;
using internship_entry_task.Models.Game.Request;
using internship_entry_task.Models.Game.Response;

namespace internship_entry_task.Application.Services.Interfaces;

/// <summary>
/// Интерфейс сервиса управления играми.
/// </summary>
public interface IGameService
{
    /// <summary>
    /// Создаёт новую игру асинхронно.
    /// </summary>
    Task<GameCreateResponse> CreateAsync();
    
    /// <summary>
    /// Возвращает игру по идентификатору из базы данных асинхронно.
    /// </summary>
    /// <param name="id">Идентификатор игры.</param>
    Task<DbGame> GetByIdAsync(Guid id);
    
    /// <summary>
    /// Асинхронно выполняет ход в игре по координатам и идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор игры.</param>
    /// <param name="moveRequest">Координаты хода.</param>
    Task<DbGame> MakeMove(Guid id, MoveRequest moveRequest);
}