using System.Data;
using Sem._5.API.Model;
using Dapper;

namespace Sem._5.API.SQLiteDB;

public class DataAccess : IDataAccess
{
    private IDbConnectionFactory _dbConnectionFactory;
    public DataAccess(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
        using (IDbConnection connection = _dbConnectionFactory.CreateConnection())
        {
            var createTableSql = @"CREATE TABLE IF NOT EXISTS Highscores (
                                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                    Time FLOAT NOT NULL,
                                    PlayerName TEXT NOT NULL,
                                    TrackName TEXT NOT NULL
                               );";
            connection.Execute(createTableSql);
        }
    }
    public void PostHighscore(Highscore highscore)
    {
        using (IDbConnection connection = _dbConnectionFactory.CreateConnection())
        {
            string insertSql = @"INSERT INTO Highscores (Time, PlayerName, TrackName) 
                                 VALUES (@Time, @PlayerName, @TrackName);";
            connection.Execute(insertSql, highscore);
        }
    }
    public IEnumerable<Highscore> GetHighscores()
    {
        using (IDbConnection connection = _dbConnectionFactory.CreateConnection())
        {
            var selectSql = @"SELECT Time, PlayerName, TrackName FROM Highscores ORDER BY Time ASC LIMIT @Count;";
            return connection.Query<Highscore>(selectSql, new { Count = 10 });
        }
    }
}