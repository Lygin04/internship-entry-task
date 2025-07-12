using internship_entry_task.Api.Controllers.V1;
using internship_entry_task.Application.Services.Interfaces;
using internship_entry_task.Domain.Entities;
using internship_entry_task.Domain.Exceptions.Game;
using internship_entry_task.Models.Game.Enums;
using internship_entry_task.Models.Game.Request;
using internship_entry_task.Models.Game.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace internship_entry_task.Tests;

public class GamesControllerTests
{
    private readonly Mock<IGameService> _gameServiceMock;
    private readonly GamesController _gameController;

    public GamesControllerTests()
    {
        _gameServiceMock = new Mock<IGameService>();
        _gameController = new GamesController(_gameServiceMock.Object)
        {
            ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            }
        };
    }

    [Fact]
    public async Task Create_ReturnsOkResult_WithGameCreateResponse()
    {
        var expectedResponse = new GameCreateResponse
        {
            Id = Guid.NewGuid(),
            N = 3,
            TurnPlayer = 'X'
        };

        _gameServiceMock.Setup(s => s.CreateAsync())
            .ReturnsAsync(expectedResponse);

        var result = await _gameController.Create();

        var okResult = Assert.IsType<OkObjectResult>(result);
        var response = Assert.IsType<GameCreateResponse>(okResult.Value);
        Assert.Equal(expectedResponse.Id, response.Id);
    }

    [Fact]
    public async Task Get_ReturnsOkResult_WhenGameExists()
    {
        var gameId = Guid.NewGuid();
        var expectedGame = new DbGame
        {
            Id = gameId,
            Board = "board",
            MoveCount = 1,
            N = 3,
            Status = GameStatus.InProgress,
            TurnPlayer = 'X'
        };

        _gameServiceMock.Setup(s => s.GetByIdAsync(gameId))
            .ReturnsAsync(expectedGame);

        var result = await _gameController.Get(gameId);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var game = Assert.IsType<DbGame>(okResult.Value);
        Assert.Equal(expectedGame.Id, game.Id);
    }

    [Fact]
    public async Task Get_ThrowsGameNotFoundException_WhenGameDoesNotExist()
    {
        var gameId = Guid.NewGuid();
        _gameServiceMock.Setup(s => s.GetByIdAsync(gameId))
            .ThrowsAsync(GameNotFoundException.WithSuchId(gameId));

        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameController.Get(gameId));
    }

    [Fact]
    public async Task MakeMove_ReturnsOkResult_WithETag()
    {
        var gameId = Guid.NewGuid();
        var moveRequest = new MoveRequest { Row = 1, Col = 1 };

        var updatedGame = new DbGame
        {
            Id = gameId,
            Board = "new board",
            MoveCount = 2,
            N = 3,
            Status = GameStatus.InProgress,
            TurnPlayer = 'O'
        };

        _gameServiceMock.Setup(s => s.MakeMove(gameId, moveRequest))
            .ReturnsAsync(updatedGame);

        var result = await _gameController.MakeMove(gameId, moveRequest);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var game = Assert.IsType<DbGame>(okResult.Value);

        Assert.Equal(updatedGame.Id, game.Id);
        var eTagWithQuotes = _gameController.Response.Headers["ETag"].ToString();
        Assert.Equal($"\"game-{updatedGame.Id}-version-{updatedGame.MoveCount}\"", eTagWithQuotes);    
    }

    [Fact]
    public async Task MakeMove_ThrowsGameNotFoundException_WhenGameDoesNotExist()
    {
        var gameId = Guid.NewGuid();
        var moveRequest = new MoveRequest { Row = 1, Col = 1 };

        _gameServiceMock.Setup(s => s.MakeMove(gameId, moveRequest))
            .ThrowsAsync(GameNotFoundException.WithSuchId(gameId));

        await Assert.ThrowsAsync<GameNotFoundException>(() => _gameController.MakeMove(gameId, moveRequest));
    }

    [Fact]
    public async Task MakeMove_ThrowsGameBadRequestException_WhenGameIsFinished()
    {
        var gameId = Guid.NewGuid();
        var moveRequest = new MoveRequest { Row = 1, Col = 1 };

        _gameServiceMock.Setup(s => s.MakeMove(gameId, moveRequest))
            .ThrowsAsync(GameBadRequestException.GameAlreadyFinished());

        await Assert.ThrowsAsync<GameBadRequestException>(() => _gameController.MakeMove(gameId, moveRequest));
    }

    [Fact]
    public async Task MakeMove_ThrowsGameBadRequestException_WhenCellAlreadyOccupied()
    {
        var gameId = Guid.NewGuid();
        var moveRequest = new MoveRequest { Row = 1, Col = 1 };

        _gameServiceMock.Setup(s => s.MakeMove(gameId, moveRequest))
            .ThrowsAsync(GameBadRequestException.CellAlreadyOccupied(1, 1));

        await Assert.ThrowsAsync<GameBadRequestException>(() => _gameController.MakeMove(gameId, moveRequest));
    }
}