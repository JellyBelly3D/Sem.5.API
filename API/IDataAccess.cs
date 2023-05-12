using Sem._5.API.Model;

namespace Sem._5.API;

public interface IDataAccess
{
    public Task<IEnumerable<Highscore>> GetHighscores(string trackName);

    public Task PostHighscore(string trackName, Highscore newScore);
}