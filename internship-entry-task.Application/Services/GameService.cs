using System.Text.Json;
using internship_entry_task.Application.Services.Interfaces;
using internship_entry_task.Domain.Entities;
using internship_entry_task.Domain.Exceptions.Game;
using internship_entry_task.Infrastructure.Dapper.Interfaces;
using internship_entry_task.Infrastructure.Repositories.Interfaces;
using internship_entry_task.Models.Game.Enums;
using internship_entry_task.Models.Game.Request;
using internship_entry_task.Models.Game.Response;

namespace internship_entry_task.Application.Services;

public class GameService : IGameService
{
    private readonly IGameRepository _gameRepository;
    private readonly IMoveRepository _moveRepository;
    private readonly IDapperContext<IDapperSettings> _dapperContext;
    private readonly int _boardSize;
    private readonly int _winCondition;
    private readonly string  _board;
    private readonly Random _rnd;
    
    public GameService(
        IGameRepository gameRepository,
        IMoveRepository moveRepository,
        IDapperContext<IDapperSettings> dapperContext)
    {
        _boardSize = int.Parse(Environment.GetEnvironmentVariable("BOARD_SIZE") ?? "3");
        _winCondition = int.Parse(Environment.GetEnvironmentVariable("WIN_CONDITION") ?? "3");
        _gameRepository = gameRepository;
        _moveRepository = moveRepository;
        _dapperContext = dapperContext;
        _rnd = new Random();
        
        var board = new char[_boardSize, _boardSize]
        ;
        for (var i = 0; i < _boardSize; i++)
        {
            for (var j = 0; j < _boardSize; j++)
            {
                board[i, j] = '▯';
            }
        }

        _board = SerializeBoard(board);
    }
    
    public async Task<GameCreateResponse> CreateAsync()
    {
        var game = new DbGame
        {
            Board = _board,
            N = _boardSize,
            MoveCount = 0,
            Status = GameStatus.InProgress
        };
        
        game.TurnPlayer = _rnd.Next(0, 2) == 1 ? 'X' : 'O';

        var gameResponse = new GameCreateResponse
        {
            Id = await _gameRepository.CreateAsync(game),
            N = game.N,
            TurnPlayer = game.TurnPlayer
        };
        
        return gameResponse;
    }

    public async Task<DbGame> GetByIdAsync(Guid id)
    {
        var game = await _gameRepository.GetByIdAsync(id);
        if (game == null)
        {
            throw GameNotFoundException.WithSuchId(id);
        }

        return game;
    }

    public async Task<DbGame> MakeMove(Guid id, MoveRequest moveRequest)
    {
        var game = await GetByIdAsync(id);

        if (game.Status is GameStatus.Draw or GameStatus.XWins or GameStatus.OWins)
        {
            throw GameBadRequestException.GameAlreadyFinished();
        }
        
        var move = new DbMove
            { GameId = id, Row = moveRequest.Row, Col = moveRequest.Col, Player = game.TurnPlayer };

        using var transaction = _dapperContext.BeginTransaction();

        try
        {
            var existingMove = await _moveRepository.ExistAsync(move, transaction);

            if (existingMove)
            {
                transaction.Commit();
                return game;
            }

            game.MoveCount++;
            
            var board = DeserializeBoard(game.Board);

            if (board[moveRequest.Col, moveRequest.Row] is 'X' or 'O')
            {
                throw GameBadRequestException.CellAlreadyOccupied(moveRequest.Row, moveRequest.Col);
            }
            
            if (game.MoveCount % 3 == 0)
            {
                if (_rnd.NextDouble() <= 0.1)
                {
                    game.TurnPlayer = game.TurnPlayer == 'O' ? 'X' : 'O';
                }
            }
            
            board[moveRequest.Col, moveRequest.Row] = game.TurnPlayer;

            if (game.MoveCount >= _winCondition * 2 - 1 && CheckWin(board, moveRequest.Row, moveRequest.Col, game.TurnPlayer))
            {
                game.Status = game.TurnPlayer == 'X' ? GameStatus.XWins : GameStatus.OWins;
            }
            else if (game.MoveCount == _boardSize * _boardSize)
            {
                game.Status = GameStatus.Draw;
            }

            game.Board = SerializeBoard(board);
            game.TurnPlayer = game.TurnPlayer == 'X' ? 'O' : 'X';
            await _moveRepository.CreateAsync(move, transaction);
            await _gameRepository.UpdateAsync(game, transaction);
            
            transaction.Commit();

            return game;
        }
        catch
        {
            transaction.Rollback();
            throw;
        }
    }

    private bool CheckWin(char[,] board, int lastRow, int lastCol, char symbol)
    {
        return CheckDirection(board, lastRow, lastCol, 0, 1, symbol) ||  // горизонталь
               CheckDirection(board, lastRow, lastCol, 1, 0, symbol) ||  // вертикаль
               CheckDirection(board, lastRow, lastCol, 1, 1, symbol) ||  // диагональ
               CheckDirection(board, lastRow, lastCol, 1, -1, symbol);   // побочная диагональ
    }

    private bool CheckDirection(char[,] board, int startRow, int startCol, int dx, int dy, char symbol)
    {
        var count = 1;
        
        count += CountInDirection(board, startRow, startCol, dx, dy, symbol);
        
        count += CountInDirection(board, startRow, startCol, -dx, -dy, symbol);

        return count >= _winCondition;
    }

    private int CountInDirection(char[,] board, int row, int col, int dx, int dy, char symbol)
    {
        var count = 0;
        var i = row + dx;
        var j = col + dy;

        while (i >= 0 && i < _boardSize && j >= 0 && j < _boardSize && board[i, j] == symbol)
        {
            count++;
            i += dx;
            j += dy;
        }

        return count;
    }
    
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
        
        var options = new JsonSerializerOptions
        {
            Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };
        
        return JsonSerializer.Serialize(jagged, options);
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