﻿using Dapper;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;

public class BaseRepository : IDisposable
{
    protected readonly MySqlConnection mySqlConnection;
    private HttpContext? _context;
    public BaseRepository(MySqlConnection mySql, IHttpContextAccessor httpContextAccessor)
    {
        mySqlConnection = mySql;
        _context = httpContextAccessor.HttpContext;
    }

    protected async Task<bool> GenericExecuteAsync(string sql, object parametros)
    {
        CreateConnection(mySqlConnection);
        return await mySqlConnection.ExecuteAsync(sql, parametros) > 0;
    }

    protected async Task<T?> GenericGetOne<T>(string sql, object? parametros)
    {
        CreateConnection(mySqlConnection);
        return await mySqlConnection.QueryFirstOrDefaultAsync<T>(sql, parametros);
    }

    protected async Task<IEnumerable<T>> GenericGetList<T>(string sql, object? parametros)
    {
        CreateConnection(mySqlConnection);
        return await mySqlConnection.QueryAsync<T>(sql, parametros);
    }

    protected bool JaExiste(string sql, object parametros)
    {
        CreateConnection(mySqlConnection);
        int count = mySqlConnection.QuerySingle<int>(sql, parametros);
        return count > 0;
    }

    protected static void CreateConnection(MySqlConnection mySqlConnection)
    {
        if (mySqlConnection.State != System.Data.ConnectionState.Open)
            mySqlConnection.Open();
    }

    protected static DateTime DataHoraAtual() => DateTime.UtcNow.AddHours(-3);
    public void Dispose()
    {
        if (mySqlConnection.State == System.Data.ConnectionState.Open)
            mySqlConnection.Close();

        mySqlConnection.Dispose();
    }
}