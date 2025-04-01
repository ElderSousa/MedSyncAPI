using Dapper;
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
        try
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                await mySqlConnection.OpenAsync();

            return await mySqlConnection.ExecuteAsync(sql, parametros) > 0;
        }
        catch
        {
            throw;
        }
    }

    protected async Task<T?> GenericGetOne<T>(string sql, object parametros)
    {
        try
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                await mySqlConnection.OpenAsync();

            return await mySqlConnection.QueryFirstOrDefaultAsync<T>(sql, parametros);
        }
        catch
        {
            throw;
        }
    }

    protected async Task<IEnumerable<T>> GenericGetList<T>(string sql, object? parametros)
    {
        try
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                await mySqlConnection.OpenAsync();

            return await mySqlConnection.QueryAsync<T>(sql, parametros);
        }
        catch
        {
            throw;
        }
    }

    protected bool JaExiste(string sql, object parametros)
    {
        try
        {
            if (mySqlConnection.State != System.Data.ConnectionState.Open)
                mySqlConnection.Open();

            return mySqlConnection.QueryFirstOrDefault<int?>(sql, parametros) > 0;
        }
        catch
        {
            throw;
        }
    }

    protected MySqlConnection CreateConnection() => new MySqlConnection(mySqlConnection.ConnectionString);
    
    protected static DateTime DataHoraAtual() => DateTime.UtcNow.AddHours(-3);
    public void Dispose()
    {
        if (mySqlConnection.State == System.Data.ConnectionState.Open)
            mySqlConnection.Close();

        mySqlConnection.Dispose();
    }
}