using internship_entry_task.Domain.Exceptions.Shared;

namespace internship_entry_task.Domain.Exceptions.Game;

/// <summary>
/// Исключение, указывающее на то, что игра не была найден.
/// </summary>
public class GameNotFoundException(string message) : NotFoundException(message)
{
    public static GameNotFoundException WithSuchId(Guid id)
    {
        return new GameNotFoundException($"Game with id {id} has not been found");
    }
}