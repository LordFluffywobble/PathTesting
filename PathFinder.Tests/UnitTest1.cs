using Testing.PathFinder;

namespace PathFinder.Tests;

public class PathTest
{
    [Fact]
    public void AddCoordinates_Should_Return_XYZ()
    {
        //Arrange: Sette opp test data og objekter
        var path = new PathFinders();
        
        //Act: Utfører selve handlingen
        path.AddCoordinates(12, 34, 56);
        
        //Assert: Sjekker at resultatet er riktig
        Assert.Equal(12, path.X);
        Assert.Equal(34, path.Y);
        Assert.Equal(56, path.Z);
    }
    
    [Fact]
    public void UpdateCoordinates_Should_Return_XYZ()
    {
        var path = new PathFinders();
        
        path.UpdateCoordinates(34, 123, 86);

        Assert.Equal( 34, path.X);
        Assert.Equal(128, path.Y);
        Assert.Equal( 68, path.Z);

    }
}
