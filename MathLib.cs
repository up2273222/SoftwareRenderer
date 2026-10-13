using System.Numerics;

namespace SoftwareRendererCS;

public static class MathLib
{
   
    public static Matrix4x4 CreateOrthographicProjectionMatrix(float left, float right, float bottom, float top, float near, float far)
    {
        Matrix4x4 outMatrix = new Matrix4x4();
        
        outMatrix.M11 = 2.0f / (right - left);
        outMatrix.M12 = 0;
        outMatrix.M13 = 0;
        outMatrix.M14 = -(right + left)/(right- left);
        
        outMatrix.M21 = 0;
        outMatrix.M22 = 2.0f / (top - bottom);
        outMatrix.M23 = 0;
        outMatrix.M24 = -(top + bottom)/(top- bottom);
        
        outMatrix.M31 = 0;
        outMatrix.M32 = 0;
        outMatrix.M33 = -2.0f / (far-near);
        outMatrix.M34 = -(far + near)/(far - near);
        
        outMatrix.M41 = 0;
        outMatrix.M42 = 0;
        outMatrix.M43 = 0;
        outMatrix.M44 = 1.0f;
        
        return outMatrix;
    }

    public static Vector3 RotateAroundAxis(Vector3 v, Vector3 axis, float angle)
    {
        angle *= MathF.PI / 180.0f;
        axis = Vector3.Normalize(axis);
        
        float cosAngle = MathF.Cos(angle);
        float sinAngle = MathF.Sin(angle);
        
        return (v * cosAngle) +
               (Cross(axis,v) * sinAngle) + 
               (axis * Dot(axis,v) * (1.0f - cosAngle));

    }
    
    public static Vector3 TransformPoint(Matrix4x4 m, Vector3 p)
    {
        Vector4 v = new Vector4(p.X, p.Y, p.Z, 1);
        Vector4 outV = Mul(m, v);
        return new Vector3(outV.X, outV.Y, outV.Z);
    }
    
    public static Vector4 Mul(Matrix4x4 m, Vector4 v)
    {
        Vector4 outV = new Vector4
        {
            X = Dot(new Vector4(m.M11,m.M12,m.M13,m.M14), v),
            Y = Dot(new Vector4(m.M21,m.M22,m.M23,m.M24),v),
            Z = Dot(new Vector4(m.M31,m.M32,m.M13,m.M34),v),
            W = Dot(new Vector4(m.M41,m.M42,m.M13,m.M44),v)
        };
        return outV;
    }
    
    public static float Dot(Vector4 a, Vector4 b)
    {
        return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z) + (a.W * b.W);
    }
    
    public static float Dot(Vector3 a, Vector3 b)
    {
        return (a.X * b.X) + (a.Y * b.Y) + (a.Z * b.Z);
    }
    
    public static Vector3 Cross(Vector3 a, Vector3 b)
    {
        return new Vector3(
            (a.Y * b.Z - a.Z * b.Y),
            (a.X * b.X - a.X * b.Z),
            (a.X * b.Y - a.Y * b.X));
    }

    public static float SignedTriArea(Triangle triangle)
    {
        return 0.5f * (((triangle.p2.Y - triangle.p1.Y) * (triangle.p2.X + triangle.p1.X) +
                       (triangle.p3.Y - triangle.p2.Y) * triangle.p3.X + triangle.p2.X)
                     + (triangle.p1.Y- triangle.p3.Y) * (triangle.p1.X + triangle.p3.X));


    }
    
    public static float SignedTriArea(float ax, float ay, float bx, float by, float cx, float cy)
    {
        return 0.5f * (((by - ay) * (bx + ax) + (cy - by) * cx + bx) + (ay- cy) * (ax + cx));


    }
}