using Testing.PathFinder.InterFaces;

namespace Testing.PathFinder;

public class PathFinders : IAddCoordinates
{
    public double X {get; set;}
    public double Y {get; set;}
    public double Z {get; set;}
    
    public void AddCoordinates(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }

    public void UpdateCoordinates(double x, double y, double z)
    {
        X = x;
        Y = y;
        Z = z;
    }
    
}
