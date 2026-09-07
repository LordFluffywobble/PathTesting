using System.Runtime.InteropServices;
using Testing.PathFinder;
using Xunit.Sdk;

namespace PathFinder.Tests;

public class PathTest
{
    [Fact]
    public void AddCoordinates_ShouldReturnXYZ()
    {
        //Arrange: Sette opp test data og objekter
        var path = new PathFinders();
        
        //Act: Utfører selve handlingen
        path.AddCoordinates(12, 34, 56);
        
        //Assert: Sjekker at resultatet er riktig
        Assert.Equal(12, path.PathHistory[0].X);
        Assert.Equal(34, path.PathHistory[0].Y);
        Assert.Equal(56, path.PathHistory[0].Z);
    }
    
    [Fact]
    public void UpdateCoordinates_ShouldReturnXYZ()
    {
        var path = new PathFinders();

        //Adding this forst otherwise the lambda function wont do anything
        path.AddCoordinates(10, 10, 10);

        //Here is the one i want to check
        path.UpdateCoordinates(34, 123, 86);

        Assert.Equal( 34, path.PathHistory[0].X);
        Assert.Equal(123, path.PathHistory[0].Y);
        Assert.Equal( 86, path.PathHistory[0].Z);
    }

    [Fact]
    public void ShortestPathTest_ShouldReturnADouble()
    {
        var path = new PathFinders();
        path.AddCoordinates(34, 56 ,23);
        path.AddCoordinates(123, 7, 23);
        
        double sPath = path.ShortestPath();
        Assert.Equal(101.597, sPath ,3);
    }

    [Fact]
    public void IncrementCoordinates_ShouldReturnADouble()
    {
        var path = new PathFinders();
        path.AddCoordinates(50, 23, 56);
        path.AddCoordinates(5, 67, 8);

        path.IncrementCoordinates(0, 0, 0);

        var updatedCoordinates = path.PathHistory[path.PathHistory.Count - 1];
        
        Assert.Equal(55, updatedCoordinates.X);
        Assert.Equal(90, updatedCoordinates.Y);
        Assert.Equal(64, updatedCoordinates.Z);
    }
}
