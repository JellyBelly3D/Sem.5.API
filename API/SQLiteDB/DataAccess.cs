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
			string createTablesQuery = @"
CREATE TABLE IF NOT EXISTS Tracks (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Name TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Scores (
	Id INTEGER PRIMARY KEY AUTOINCREMENT,
	Time FLOAT NOT NULL,
	PlayerName TEXT NOT NULL,
  	TrackId INTEGER NOT NULL,
  	FOREIGN KEY (TrackId) REFERENCES Tracks(Id)
);				
";
			connection.Execute(createTablesQuery);
		}
	}

	public async Task<IEnumerable<Highscore>> GetHighscores(string trackName)
	{
		using (IDbConnection connection = _dbConnectionFactory.CreateConnection())
		{
			return await GetHighscoresQuery(trackName, connection);
		}
	}

	// This is separated out so the PostHighscore method can use it in its transaction
	private async Task<IEnumerable<Highscore>> GetHighscoresQuery(string trackName, IDbConnection connection)
	{
		string selectQuery = @"
SELECT Scores.PlayerName, Scores.Time
FROM Scores
JOIN Tracks ON Scores.TrackId = Tracks.Id
WHERE Tracks.Name = @TrackName
ORDER BY Scores.Time ASC
LIMIT @Count;
";

		return await connection.QueryAsync<Highscore>(selectQuery, new { TrackName = trackName, Count = 10 });
	}

	// First starts a transaction
	// Then reads the top 10 for the given trackName and determines if the submitted score should be posted
	// Then it tries to find the id for the given track, if it doesn't then the track doesn't exist and it creates a new one
	// Then it inserts the score with the previously found/created track id
	// Lastly it commits the transaction if only 1 row was changed by the insert
	public async Task PostHighscore(string trackName, Highscore newScore)
	{
		using (IDbConnection connection = _dbConnectionFactory.CreateConnection())
		{
			using (IDbTransaction transaction = connection.BeginTransaction())
			{
				IEnumerable<Highscore> highscores = await GetHighscoresQuery(trackName, connection);
				if (!IsNewHighscore(newScore, highscores)) return;

				string checkTrackQuery = "SELECT Id FROM Tracks WHERE Name = @Name";
				int? trackId =
					await connection.QuerySingleOrDefaultAsync<int?>(checkTrackQuery, new { Name = trackName },
						transaction);

				if (trackId == null)
				{
					// Create a new track
					string createTrackQuery = "INSERT INTO Tracks (Name) VALUES (@Name); SELECT last_insert_rowid()";
					trackId = await connection.QuerySingleAsync<int>(createTrackQuery, new { Name = trackName },
						transaction);
				}

				// Insert the score
				string insertScoreQuery =
					"INSERT INTO Scores (Time, PlayerName, TrackId) VALUES (@Time, @PlayerName, @TrackId)";
				int changedRows = await connection.ExecuteAsync(insertScoreQuery,
					new { Time = newScore.Time, PlayerName = newScore.PlayerName, TrackId = trackId }, transaction);

				if (changedRows == 1)
				{
					transaction.Commit();
				}
				else
				{
					transaction.Rollback();
				}
			}
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