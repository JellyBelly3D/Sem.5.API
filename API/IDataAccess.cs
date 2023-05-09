using Sem._5.API.Model;

namespace Sem._5.API;

public interface IDataAccess
{
    public Task<IEnumerable<Highscore>> GetHighscores();

    public Task PostHighscore(Highscore newScore);
}