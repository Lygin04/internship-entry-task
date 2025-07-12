namespace internship_entry_task.Models.Game.Enums;

public enum GameStatus
{
    /// <summary>
    /// Игра еще не закончена.
    /// </summary>
    InProgress,
    
    /// <summary>
    /// Игра закончилась ничьей.
    /// </summary>
    Draw,
    
    /// <summary>
    /// Игра закончилась победой X.
    /// </summary>
    XWins,
    
    /// <summary>
    /// Игра закончилась победой O.
    /// </summary>
    OWins
}