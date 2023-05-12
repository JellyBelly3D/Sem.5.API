using Sem._5.API.Model;

namespace Sem._5.API.MockDB;

public class DataAccessMock : IDataAccess
{
	private Dictionary<string, List<Highscore>> _mockScores = new Dictionary<string, List<Highscore>>();

	public async Task<IEnumerable<Highscore>> GetHighscores(string trackName)
	{
		if (!_mockScores.TryGetValue(trackName, out List<Highscore> trackScores))
		{
			trackScores = new List<Highscore>();
		}

		return trackScores;
	}

	public async Task PostHighscore(string trackName, Highscore newScore)
	{
		if (_mockScores.TryGetValue(trackName, out List<Highscore> trackScores))
		{
			trackScores.Add(newScore);
		}
		else
		{
			trackScores = new List<Highscore>();
			trackScores.Add(newScore);
			_mockScores.Add(trackName, trackScores);
		}
	}
}