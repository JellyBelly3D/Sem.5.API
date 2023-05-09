using Sem._5.API.Model;

namespace Sem._5.API;

public interface IDataAccess
{
    public IEnumerable<Highscore> GetHighscores();

    public void PostHighscore(Highscore highscore);
}