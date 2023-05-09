using Sem._5.API.Model;
using Dapper;
using Microsoft.Data.Sqlite;
using System.Data.SQLite;
namespace Sem._5.API.SQLiteDB;

public class DataAccess : IDataAccess
{
    private readonly SqliteConnection _connection;

    public DataAccess()
    {
        _connection = new SqliteConnection("Data Source=highscores.db;");
        _connection.Open();
        
        var createTableSql = @"CREATE TABLE IF NOT EXISTS Highscores (
                                   Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                   Time FLOAT NOT NULL,
                                   PlayerName TEXT NOT NULL,
                                   TrackName TEXT NOT NULL
                              );";
        _connection.Execute(createTableSql);
    }
    public void PostHighscore(Highscore highscore)
    {
        string insertSql = @"INSERT INTO Highscores (Time, PlayerName, TrackName) 
                             VALUES (@Time, @PlayerName, @TrackName);";
        _connection.Execute(insertSql, highscore);
    }
    public IEnumerable<Highscore> GetHighscores()
    {
        var selectSql = @"SELECT Time, PlayerName, TrackName FROM Highscores ORDER BY Time ASC LIMIT @Count;";
        return _connection.Query<Highscore>(selectSql, new { Count = 10 });
    }
    
    public void Dispose()
    {
        _connection.Dispose();
    }
}