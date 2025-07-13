﻿namespace internship_entry_task.Infrastructure.Dapper.Interfaces;

public interface ITransaction : IDisposable
{
    Task<T?> CommandWithResponse<T>(IQueryObject queryObject);
    Task Command(IQueryObject queryObject);
    public void Commit();
    public void Rollback();
}