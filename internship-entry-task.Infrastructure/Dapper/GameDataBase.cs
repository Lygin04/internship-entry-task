using internship_entry_task.Infrastructure.Dapper.Interfaces;
using internship_entry_task.Infrastructure.Dapper.Models;
using Microsoft.Extensions.Configuration;

namespace internship_entry_task.Infrastructure.Dapper;

public class GameDataBase : IDapperSettings
{
    private readonly IConfiguration _configuration;
    
    public GameDataBase(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public string ConnectionString => _configuration["GameDataBase:ConnectionString"] ?? string.Empty;
    public Provider Provider => Enum.Parse<Provider>(_configuration["GameDataBase:Provider"] ?? string.Empty);
}