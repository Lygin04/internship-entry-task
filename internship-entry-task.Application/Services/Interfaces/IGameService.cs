using internship_entry_task.Domain.Entities;
using internship_entry_task.Models.Game.Request;
using internship_entry_task.Models.Game.Response;

namespace internship_entry_task.Application.Services.Interfaces;

public interface IGameService
{
    Task<GameCreateResponse> CreateAsync();
    Task<DbGame> GetByIdAsync(Guid id);
    Task<DbGame> MakeMove(Guid id, MoveRequest moveRequest);
}