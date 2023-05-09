using Microsoft.AspNetCore.Mvc;
using Sem._5.API.MockDB;
using Sem._5.API.Model;

namespace Sem._5.API;

public static class Endpoints
{
    public static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/highscores", Get);
        app.MapPost("/highscores", Create);
    }
    
    private static IEnumerable<Highscore> Get(IDataAccess dataAccess)
    {
        IEnumerable<Highscore> highscores = dataAccess.GetHighscores();
        return highscores;
    }
    
    private static async Task<IResult> Create(IDataAccess dataAccess, [FromBody] Highscore highscore)
    {
        IEnumerable<Highscore> highscores = Get(dataAccess);
        
        dataAccess.PostHighscore(highscore);
        return Results.Ok();
    }
}