using System.Numerics;

namespace SoftwareRendererCS;

public class Triangle
{
    
        public Vector3 p1, p2, p3;

        public Triangle(Vector3 p1, Vector3 p2, Vector3 p3)
        {
            this.p1 = p1;
            this.p2 = p2;
            this.p3 = p3;
        }
        
}