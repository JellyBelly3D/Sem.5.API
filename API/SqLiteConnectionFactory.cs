using System.Data;
using Microsoft.Data.Sqlite;

namespace Sem._5.API;
public class SqLiteConnectionFactory : IDbConnectionFactory
{
    private readonly string? _connectionString;

    public SqLiteConnectionFactory(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("SQLite");
    }
    public IDbConnection CreateConnection()
    {
        var connection = new SqliteConnection(_connectionString);
        connection.Open();
        return connection;
    }
}