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
            using var connection = CreateConnection();
            await connection.OpenAsync();
            return await connection.ExecuteAsync(sql, parametros) > 0;
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
            using var connection = CreateConnection();
            await connection.OpenAsync();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parametros);
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
            using var connection = CreateConnection();
            await connection.OpenAsync();
            return await connection.QueryAsync<T>(sql, parametros);
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
            using var connection = CreateConnection();
            connection.Open();
            return connection.QueryFirstOrDefault<int?>(sql, parametros) > 0;
        }
        catch
        {
            throw;
        }
    }

    protected MySqlConnection CreateConnection() => new MySqlConnection(mySqlConnection.ConnectionString);
    
    protected static DateTime DataHoraAtual() => DateTime.UtcNow.AddHours(-3);
    public void Dispose() => mySqlConnection.Dispose();
}