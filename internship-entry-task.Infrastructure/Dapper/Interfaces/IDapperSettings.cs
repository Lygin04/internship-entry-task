using internship_entry_task.Infrastructure.Dapper.Models;

namespace internship_entry_task.Infrastructure.Dapper.Interfaces;

public interface IDapperSettings
{
    string ConnectionString { get; }
    Provider Provider { get; }
}