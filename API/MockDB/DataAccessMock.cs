using Sem._5.API.Model;
namespace Sem._5.API.MockDB;

public class DataAccessMock : IDataAccess
{ 
    List<Highscore> mockScores = new List<Highscore>();
    
    public async Task<IEnumerable<Highscore>> GetHighscores()
    {
        return mockScores;
    }

    public async Task PostHighscore(Highscore newScore)
    {
        mockScores.Add(newScore);
    }
}