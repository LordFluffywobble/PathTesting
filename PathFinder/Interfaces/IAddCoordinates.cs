using System.Security.Cryptography.X509Certificates;

namespace Testing.PathFinder.InterFaces;

public interface IAddCoordinates
{
    double X {get; set;}
    double Y {get; set;}
    double Z {get; set;}
}