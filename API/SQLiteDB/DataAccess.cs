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
    public async Task<IEnumerable<Highscore>> GetHighscores()
    {
        using (IDbConnection connection = _dbConnectionFactory.CreateConnection())
        {
            var selectSql = @"SELECT Time, PlayerName, TrackName FROM Highscores ORDER BY Time ASC LIMIT @Count;";
            return await connection.QueryAsync<Highscore>(selectSql, new { Count = 10 });
        }
    }
    public async Task PostHighscore(Highscore newScore)
    {
        IEnumerable<Highscore> highscores = await GetHighscores();
        if (!IsNewHighscore(newScore, highscores)) return;
        
        Console.WriteLine("NEW HIGHSCORE!");
        
        using (IDbConnection connection = _dbConnectionFactory.CreateConnection())
        {
            string insertSql = @"INSERT INTO Highscores (Time, PlayerName, TrackName) 
                                 VALUES (@Time, @PlayerName, @TrackName);";
            await connection.ExecuteAsync(insertSql, newScore);
        }
    }
    public static bool IsNewHighscore(Highscore newScore, IEnumerable<Highscore> highscores)
    {
        if (newScore.Time <= 0) return false;
        
        bool isTotalScoresLessThan10 = highscores.Count() >= 10;
        bool isNewScoreBetterThanTop10 = highscores.Any(highscore => newScore.Time < highscore.Time);
        
        if (isTotalScoresLessThan10 && !isNewScoreBetterThanTop10) return false;
        
        return true;
    }
}