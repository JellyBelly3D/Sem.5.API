using Microsoft.AspNetCore.Mvc;
using Sem._5.API.MockDB;
using Sem._5.API.Model;

namespace Sem._5.API;

public static class Endpoints
{
    public static void AddEndpoints(this WebApplication app)
    {
        app.MapGet("/highscores/{trackName}", Get);
        app.MapPost("/highscores/{trackName}", Create);
    }
    
    private static async Task<IEnumerable<Highscore>> Get(IDataAccess dataAccess, string trackName)
    {
        IEnumerable<Highscore> highscores = await dataAccess.GetHighscores(trackName);
        return highscores;
    }
    
    private static async Task<IResult> Create(IDataAccess dataAccess, string trackName, [FromBody] Highscore newScore)
    {
        Console.WriteLine("I here");
        await dataAccess.PostHighscore(trackName, newScore);
        return Results.Ok();
    }
}