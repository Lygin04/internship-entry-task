﻿using internship_entry_task.Infrastructure.Dapper.Interfaces;

namespace internship_entry_task.Infrastructure.Dapper.Models;

public class QueryObject : IQueryObject
{
    public QueryObject(string sql, object? parameters = null, int commandTimeout = 30)
    {
        if (string.IsNullOrEmpty(sql))
        {
            throw new ArgumentNullException(nameof(sql));
        }

        Sql = sql;
        Params = parameters;
        CommandTimeout = commandTimeout;
    }

    public string Sql { get; }
    public object? Params { get; }
    public int CommandTimeout { get; }
}