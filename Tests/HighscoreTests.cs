using Xunit;
using Sem._5.API.Model;
using Sem._5.API.SQLiteDB;

namespace Sem._5.API.TestProject;

public class HighscoreTests
{
    [Theory]
    [InlineData(0)]
    [InlineData(9)]
    [InlineData(10)]
    public void AddBetterScoreTest(int highscoreAmount)
    {
        // Arrange
        List<Highscore> testHighscores = new List<Highscore>();
        
        Highscore scoreData = new Highscore
        {
            Time = 2,
            PlayerName = "Test",
            TrackName = "Test"
        };
        
        Highscore newBestScore = new Highscore
        {
            Time = 1,
            PlayerName = "Test",
            TrackName = "Test"
        };
        
        for(int i = 0; i < highscoreAmount; i++)
        {
            testHighscores.Add(scoreData);
        }
        
        // Act/Assert
        Assert.True(DataAccess.IsNewHighscore(newBestScore, testHighscores));
    }
    
    [Theory]
    [InlineData(10,2)]
    [InlineData(10,3)]
    public void AddWorseScoreToFullTop10(int highscoreAmount, int worseScore)
    {
        // Arrange
        List<Highscore> testHighscores = new List<Highscore>();
        
        Highscore scoreData = new Highscore
        {
            Time = 2,
            PlayerName = "Test",
            TrackName = "Test"
        };
        
        Highscore newScore = new Highscore
        {
            Time = worseScore,
            PlayerName = "Test",
            TrackName = "Test"
        };
        
        for(int i = 0; i < highscoreAmount; i++)
        {
            testHighscores.Add(scoreData);
        }
        
        // Act/Assert
        Assert.False(DataAccess.IsNewHighscore(newScore, testHighscores));
    }
    
    [Theory]
    [InlineData(0)]
    [InlineData(9)]
    public void AddWorseScoreToNotFullTop10(int highscoreAmount)
    {
        // Arrange
        List<Highscore> testHighscores = new List<Highscore>();
        
        Highscore scoreData = new Highscore
        {
            Time = 2,
            PlayerName = "Test",
            TrackName = "Test"
        };
        
        Highscore newBestScore = new Highscore
        {
            Time = 3,
            PlayerName = "Test",
            TrackName = "Test"
        };
        
        for(int i = 0; i < highscoreAmount; i++)
        {
            testHighscores.Add(scoreData);
        }
        
        // Act/Assert
        Assert.True(DataAccess.IsNewHighscore(newBestScore, testHighscores));
    }
}