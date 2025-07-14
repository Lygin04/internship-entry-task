using internship_entry_task.Api.Controllers.Abstract;
using internship_entry_task.Application.Services.Interfaces;
using internship_entry_task.Models.Game.Request;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;

namespace internship_entry_task.Api.Controllers.V1;

public class GamesController(IGameService gameService) : ApiControllerV1
{
    [HttpPost("Create")]
    public async Task<IActionResult> Create()
    {
        var id = await gameService.CreateAsync();
        return Ok(id);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var game = await gameService.GetByIdAsync(id);
        return Ok(game);
    }

    [HttpPost("{id}/move")]
    public async Task<IActionResult> MakeMove(Guid id, MoveRequest moveRequest)
    {
        var game = await gameService.MakeMove(id, moveRequest);
        
        var eTag = $"\"game-{game.Id}-version-{game.MoveCount}\"";
        Response.Headers[HeaderNames.ETag] = eTag;
        
        return Ok(game);
    }
}