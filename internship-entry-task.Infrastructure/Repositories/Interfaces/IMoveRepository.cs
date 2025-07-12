using internship_entry_task.Domain.Entities;
using internship_entry_task.Infrastructure.Dapper.Interfaces;

namespace internship_entry_task.Infrastructure.Repositories.Interfaces;

public interface IMoveRepository
{
    Task CreateAsync(DbMove move, ITransaction? transaction = null);
    Task<bool> ExistAsync(DbMove move, ITransaction? transaction = null);
}