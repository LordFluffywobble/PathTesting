
using System.Data.Common;
using System.Security.Cryptography.X509Certificates;

namespace Testing.PathFinder;

public record struct Coordinates(double X, double Y, double Z);
public class PathFinders
{
  
    public List<Coordinates> PathHistory {get;}
    public PathFinders() => PathHistory = new List<Coordinates>();
    public void AddCoordinates(double x, double y, double z) => 
        PathHistory.Add(new Coordinates(x, y, z));
    

    public void UpdateCoordinates(double x, double y, double z, int id) => 
        PathHistory[id] = new Coordinates(x, y, z) ;
    
    public double ShortestPath(int id1, int id2)
    {

        var path1 = PathHistory[id1];
        var path2 = PathHistory[id2];

        double dx = path2.X - path1.X;
        double dy = path2.Y - path1.Y;
        double dz = path2.Z - path1.Z;

        return Math.Sqrt(dx * dx + dy * dy + dz * dz);
    }

    public void IncrementCoordinates(double x, double y, double z, int id)
    {
        var path1 = PathHistory[id];
        
        var newCoordinate = new Coordinates(
            path1.X + x,
            path1.Y + y,
            path1.Z + z
        );

        PathHistory.Add(newCoordinate);     
    }
    public void DeleteTaskId(int id) {
        PathHistory.RemoveAt(id);
    }
}
