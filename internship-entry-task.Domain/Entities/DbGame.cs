using System.Text.Json;
using System.Text.Json.Serialization;
using internship_entry_task.Models.Game.Enums;

namespace internship_entry_task.Domain.Entities;

public class DbGame
{
    /// <summary>
    /// 
    /// </summary>
    public Guid Id { get; set; }
    
    /// <summary>
    /// Поле игры.
    /// </summary>
    public string Board { get; set; }
    
    /// <summary>
    /// Количество строк и столбцов.
    /// </summary>
    public int N { get; set; }
    
    /// <summary>
    /// Счетчик ходов.
    /// </summary>
    public int MoveCount { get; set; }
    
    /// <summary>
    /// Чей сейчас ход.
    /// </summary>
    public char TurnPlayer { get; set; }
    
    /// <summary>
    /// Статус игры.
    /// </summary>
    public GameStatus Status { get; set; }
    
    
    private static string SerializeBoard(char[,] board)
    {
        int rows = board.GetLength(0);
        int cols = board.GetLength(1);
        var jagged = new string[rows][];
        for (int i = 0; i < rows; i++)
        {
            jagged[i] = new string[cols];
            for (int j = 0; j < cols; j++)
            {
                jagged[i][j] = board[i, j].ToString();
            }
        }

        return JsonSerializer.Serialize(jagged);
    }

    private static char[,] DeserializeBoard(string json)
    {
        var jagged = JsonSerializer.Deserialize<string[][]>(json);
        int rows = jagged.Length;
        int cols = jagged[0].Length;
        var board = new char[rows, cols];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                board[i, j] = jagged[i][j][0];
            }
        }

        return board;
    }
}