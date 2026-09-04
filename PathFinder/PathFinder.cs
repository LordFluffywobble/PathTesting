
namespace Testing.PathFinder;

public record struct Coordinates(double X, double Y, double Z);
public class PathFinders
{
  
    public List<Coordinates> PathHistory {get;}
    public PathFinders() => PathHistory = new List<Coordinates>();
    public void AddCoordinates(double x, double y, double z) => 
        PathHistory.Add(new Coordinates(x, y, z));
    

    public void UpdateCoordinates(double x, double y, double z) => 
        _ = PathHistory.Count > 0 ? PathHistory[PathHistory.Count - 1] = new Coordinates(x, y, z) : default;
    
    public double ShortestPathTest()
    {

        if (PathHistory.Count < 2) 
            return 0;
        var path1 = PathHistory[0];
        var path2 = PathHistory[1];

        double dx = path2.X - path1.X;
        double dy = path2.Y - path1.Y;
        double dz = path2.Z - path1.Z;

        return Math.Sqrt(dx * dx + dy * dy + dz * dz);

    }
}
