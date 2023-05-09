using Sem._5.API.Model;
namespace Sem._5.API.MockDB;

public class DataAccessMock : IDataAccess
{ 
    List<Highscore> mockScores = new List<Highscore>();
    
    public IEnumerable<Highscore> GetHighscores()
    {
        return mockScores;
    }

    public void PostHighscore(Highscore highscore)
    {
        mockScores.Add(highscore);
    }
}