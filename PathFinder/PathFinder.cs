using System.Dynamic;
using Testing.PathFinder.InterFaces;

namespace Testing.PathFinder;

public record struct Coordinates(double X, double Y, double Z);
public class PathFinders : IAddCoordinates
{
    
    public double X {get; set;}
    public double Y {get; set;}
    public double Z {get; set;}
    
    public List<Coordinates> PathHistory {get;}
    public PathFinders() => PathHistory = new List<Coordinates>();
    public void AddCoordinates(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;

        PathHistory.Add(new Coordinates(x, y, z));
    }

    public void UpdateCoordinates(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
}
