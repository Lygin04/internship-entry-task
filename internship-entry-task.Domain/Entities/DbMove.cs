namespace internship_entry_task.Domain.Entities;

/// <summary>
/// Представляет сущность хода.
/// </summary>
public class DbMove
{
    /// <summary>
    /// Получает или задает идентификатор хода.
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Получает или задает идентификатор игры к которой относится ход.
    /// </summary>
    public Guid GameId { get; set; }
    
    /// <summary>
    /// Получает или задает ряд где был сделан ход.
    /// </summary>
    public int Row { get; set; }
    
    /// <summary>
    /// Получает или задает колонку где был сделан ход.
    /// </summary>
    public int Col { get; set; }
    
    /// <summary>
    /// Получает или задает игрока которым был сделан ход.
    /// </summary>
    public char Player { get; set; }
}