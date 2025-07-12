using internship_entry_task.Domain.Entities;
using internship_entry_task.Infrastructure.Dapper.Interfaces;

namespace internship_entry_task.Infrastructure.Repositories.Interfaces;

public interface IGameRepository
{
    Task<Guid> CreateAsync(DbGame game, ITransaction? transaction = null);
    Task UpdateAsync(DbGame game, ITransaction? transaction = null);
    Task<DbGame?> GetByIdAsync(Guid id);
}