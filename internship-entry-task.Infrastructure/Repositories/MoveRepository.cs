using internship_entry_task.Domain.Entities;
using internship_entry_task.Infrastructure.Dapper.Interfaces;
using internship_entry_task.Infrastructure.Dapper.Models;
using internship_entry_task.Infrastructure.Repositories.Interfaces;
using internship_entry_task.Infrastructure.Repositories.Scripts.Move;

namespace internship_entry_task.Infrastructure.Repositories;

public class MoveRepository(IDapperContext<IDapperSettings> dapperContext) : IMoveRepository
{
    public async Task CreateAsync(DbMove move, ITransaction? transaction = null)
    {
        await dapperContext.Command(new QueryObject(Move.Create, move), transaction);
    }

    public async Task<bool> ExistAsync(DbMove move, ITransaction? transaction = null)
    {
        return await dapperContext.CommandWithResponse<bool>(new QueryObject(Move.ExistMove, move), transaction);
    }
}