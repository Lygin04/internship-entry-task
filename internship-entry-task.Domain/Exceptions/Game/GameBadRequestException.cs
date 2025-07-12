using System.Data;
using internship_entry_task.Domain.Exceptions.Shared;

namespace internship_entry_task.Domain.Exceptions.Game;

public class GameBadRequestException(string message) : BadRequestException(message)
{
    public static GameBadRequestException GameAlreadyFinished()
    {
        return new GameBadRequestException("Game already finished");
    }

    public static GameBadRequestException CellAlreadyOccupied(int row, int col)
    {
        return new GameBadRequestException($"Cell {row};{col} already occupied");
    }
}