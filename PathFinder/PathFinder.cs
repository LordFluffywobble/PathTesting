
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
    
    public void ShortestPathTest(double x, double y, double z)
    {
        
    }
}
