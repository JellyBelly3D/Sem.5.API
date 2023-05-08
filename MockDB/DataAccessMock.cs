using Sem._5.API.Model;
namespace Sem._5.API.MockDB;

public class DataAccessMock : IDataAccess
{ 
    List<Highscore> mockScores = new List<Highscore>();
    
    public List<Highscore> GetHighscores()
    {
        return mockScores;
    }

    public bool PostHighscore(Highscore highscore)
    {
        mockScores.Add(highscore);
        return true;
    }
}