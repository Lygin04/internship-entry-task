using System.Text.Json;
using System.Text.Json.Serialization;
using internship_entry_task.Models.Game.Enums;

namespace internship_entry_task.Domain.Entities;

/// <summary>
/// Представляет сущность игры.
/// </summary>
public class DbGame
{
    /// <summary>
    /// Получает или задает идентификатор игры
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Получает или задает поле игры.
    /// </summary>
    public string Board { get; set; }
    
    /// <summary>
    /// Получает или задает количество строк и столбцов.
    /// </summary>
    public int N { get; set; }
    
    /// <summary>
    /// Получает или задает счетчик ходов.
    /// </summary>
    public int MoveCount { get; set; }
    
    /// <summary>
    /// Получает или задает чей сейчас ход.
    /// </summary>
    public char TurnPlayer { get; set; }
    
    /// <summary>
    /// Получает или задает статус игры.
    /// </summary>
    public GameStatus Status { get; set; }
}