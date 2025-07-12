using internship_entry_task.Domain.Entities;
using internship_entry_task.Infrastructure.Dapper.Interfaces;
using internship_entry_task.Infrastructure.Dapper.Models;
using internship_entry_task.Infrastructure.Repositories.Interfaces;
using internship_entry_task.Infrastructure.Repositories.Scripts.Game;

namespace internship_entry_task.Infrastructure.Repositories;

public class GameRepository(IDapperContext<IDapperSettings> dapperContext) : IGameRepository
{
    public async Task<Guid> CreateAsync(DbGame game, ITransaction? transaction = null)
    {
        return await dapperContext.CommandWithResponse<Guid>(new QueryObject(Game.Create, game), transaction);
    }

    public async Task UpdateAsync(DbGame game, ITransaction? transaction = null)
    {
        await dapperContext.Command(new QueryObject(Game.Update,  game), transaction);
    }

    public async Task<DbGame?> GetByIdAsync(Guid id)
    {
        return await dapperContext.FirstOrDefault<DbGame>(new QueryObject(Game.GetById, new { id }));
    }
    
}