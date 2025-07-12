using internship_entry_task.Api.Extensions;
using internship_entry_task.Api.Middleware;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using HealthChecks.UI.Client;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddHealthChecks()
    .AddNpgSql(connectionString: configuration["GameDataBase:ConnectionString"],
        name:"postgresql",
        timeout: TimeSpan.FromSeconds(5),
        tags: ["db", "sql", "postgres"]);

builder.Services.AddRouting(options => options.LowercaseUrls = true);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddDapper();
builder.Services.MigrateDatabase(configuration);

builder.Services.AddInfrastructure();
builder.Services.AddApplication();
builder.Services.AddSwagger();
builder.Services.AddTransient<ExceptionHandlingMiddleware>();

var app = builder.Build();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.MapControllers();

app.Run();

public class ProgramPlaceholder { }