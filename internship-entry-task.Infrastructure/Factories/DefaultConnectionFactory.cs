using System.Data.Common;
using internship_entry_task.Infrastructure.Factories.Interfaces;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace internship_entry_task.Infrastructure.Factories;

public class DefaultConnectionFactory : IDbConnectionFactory
{
    private readonly IConfiguration _configuration;
    
    public DefaultConnectionFactory(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    public async Task<DbConnection> CreateAsync()
    {
        var connection = new NpgsqlConnection(_configuration.GetConnectionString("Default"));
        await connection.OpenAsync();
        return connection;
    }
}