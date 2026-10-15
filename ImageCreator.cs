using System.Diagnostics;
using System.Numerics;
using System.IO;
using Microsoft.VisualBasic.CompilerServices;
using Raylib_cs;
using static SoftwareRendererCS.MathLib;

namespace SoftwareRendererCS;

public static class ImageCreator
{
    public static Vector3[,] image;
    public static Vector3[,] depthBuffer;

    
    public static byte[] imageBytes;

    private static Random rand;

    public static void InitByteArray()
    {
        imageBytes = new byte[800 * 800 * 4];
        rand = new Random();
    }

    public static void ClearFrame()
    {
        for (int y = 0; y < image.GetLength(1); y++)
        {
            for (int x = 0; x < image.GetLength(0); x++)
            {
               image[x, y] = Vector3.Zero;
            }
        }
    }

    public static void WriteImageToFile()
    {
        string path = "C:/Users/archi/Documents/SoftwareRenderer/PSGAMCS/SoftwareRendererCS/Image.bmp";
        using BinaryWriter writer = new(File.Open(path, FileMode.Create));
        uint[] ByteOffset = {14,40,(uint)image.Length * 4};
    
        //BMP Image File Header
        //Bitmap identifier
        writer.Write((byte)'B');
        writer.Write((byte)'M');
        //File Size
        writer.Write(ByteOffset[0] + ByteOffset[1] + ByteOffset[2]);
        //Reserved 
        writer.Write(new byte[4]);
       
        //Offset to when data starts (byte offset - filesize)
        writer.Write(ByteOffset[0] + ByteOffset[1]);
        
        //DIB header (not sure what that is but its 40 bytes)
        writer.Write(ByteOffset[1]);
        //Width,height
        writer.Write((int)image.GetLength(0));
        writer.Write((int)image.GetLength(1));
        //Color settings?
        writer.Write((ushort)1);
        writer.Write((ushort)(8 * 4));
        //Compression amount (0 - ignore)
        writer.Write(new byte[4]);
        //File size
        writer.Write(ByteOffset[2]);
        //Bunch of bullshit?
        writer.Write(new byte[16]);
       

        

        for (int y = 0; y < image.GetLength(1); y++)
        {
            for (int x = 0; x < image.GetLength(0); x++)
            {
                Vector3 col = image[x, y];
                writer.Write((byte)(col.Z * 255));
                writer.Write((byte)(col.Y * 255));
                writer.Write((byte)(col.X * 255));
                writer.Write((byte)255);
            }
        }
    
       
    

    }

    public static void FillTriangle(Triangle triangle)
    {
        
        float scaledP1x = ((triangle.p1.X + 1.0f) / 2.0f) * 800f;
        float scaledP1y = ((triangle.p1.Y + 1.0f) / 2.0f) * 800f;
        float scaledP1z = ((triangle.p1.Z + 1.0f) / 2.0f) * 800f;
       // float scaledP1w = ((triangle.p1.W + 1.0f) / 2.0f) * 800f;
        
        float scaledP2x = ((triangle.p2.X + 1.0f) / 2.0f) * 800f;
        float scaledP2y = ((triangle.p2.Y + 1.0f) / 2.0f) * 800f;
        float scaledP2z = ((triangle.p2.Z + 1.0f) / 2.0f) * 800f;
      //  float scaledP2w = ((triangle.p1.W + 1.0f) / 2.0f) * 800f;
        
        float scaledP3x = ((triangle.p3.X + 1.0f) / 2.0f) * 800f;
        float scaledP3y = ((triangle.p3.Y + 1.0f) / 2.0f) * 800f;
        float scaledP3z = ((triangle.p3.Z + 1.0f) / 2.0f) * 800f;
      //  float scaledP3w = ((triangle.p1.W + 1.0f) / 2.0f) * 800f;
        
        Vector3 scaledP1 = new Vector3(scaledP1x, scaledP1y, scaledP1z);
        Vector3 scaledP2 = new Vector3(scaledP2x, scaledP2y, scaledP2z);
        Vector3 scaledP3 = new Vector3(scaledP3x, scaledP3y, scaledP3z);
        
        Triangle scaledTriangle = new Triangle(scaledP1, scaledP2, scaledP3);

        int bbminx = (int)MathF.Round(Math.Min(Math.Min(scaledP1.X, scaledP2.X), scaledP3.X), MidpointRounding.AwayFromZero);
        int bbmaxx = (int)MathF.Round(Math.Max(Math.Max(scaledP1.X,scaledP2.X),scaledP3.X), MidpointRounding.AwayFromZero);
        int bbminy = (int)MathF.Round(Math.Min(Math.Min(scaledP1.Y,scaledP2.Y),scaledP3.Y), MidpointRounding.AwayFromZero);
        int bbmaxy = (int)MathF.Round(Math.Max(Math.Max(scaledP1.Y,scaledP2.Y),scaledP3.Y), MidpointRounding.AwayFromZero);
        
    
  
        

        float totalArea = SignedTriArea(scaledTriangle);
        
        if (totalArea < 1.0f) { return;}

        for (int x = bbminx; x <= bbmaxx; x++)
        {
            for (int y = bbminy; y <= bbmaxy; y++)
            {
                float a = SignedTriArea(x,y,scaledP2.X,scaledP2.Y,scaledP3.X,scaledP3.Y)/totalArea;
                float b = SignedTriArea(x,y, scaledP3.X,scaledP3.Y,scaledP1.X,scaledP1.Y)/totalArea;
                float c = SignedTriArea(x,y,scaledP1.X,scaledP1.Y,scaledP2.X,scaledP2.Y)/totalArea;

                
                
                if (a < 0 || b < 0 || c < 0) { continue; }
                SetPixel(x,y,Color.Red);
            }
        }
        




    }
    
    
     public static void DrawDDATriangle(Triangle triangle)
    {
        DrawDDALine(triangle.p1.X, triangle.p1.Y, triangle.p2.X, triangle.p2.Y);
        DrawDDALine(triangle.p2.X, triangle.p2.Y, triangle.p3.X, triangle.p3.Y);
        DrawDDALine(triangle.p3.X, triangle.p3.Y, triangle.p1.X, triangle.p1.Y);
    }
    
    public static void DrawDDATriangle(Vector3 p1, Vector3 p2, Vector3 p3)
    {
        DrawDDALine(p1.X, p1.Y, p2.X, p2.Y);
        DrawDDALine(p2.X, p2.Y, p3.X, p3.Y);
        DrawDDALine(p3.X, p3.Y, p1.X, p1.Y);
    }

    public static void DrawDDALine(float x1, float y1, float x2, float y2)
    {
        float scaledX1 = ((x1 + 1.0f) / 2.0f) * 800f;
        float scaledX2 = ((x2 + 1.0f) / 2.0f) * 800f;
        float scaledY1 = ((y1 + 1.0f) / 2.0f) * 800f;
        float scaledY2 = ((y2 + 1.0f) / 2.0f) * 800f;
      
      
        var dx = scaledX2 - scaledX1;
        var dy = scaledY2 - scaledY1;

        float step;

        if (MathF.Abs(dx) >= MathF.Abs(dy))
        {
            step = MathF.Abs(dx);
        }
        else
        {
            step = MathF.Abs(dy);
        }
        
        dx /= step;
        dy /= step;

        var x = scaledX1;
        var y = scaledY1;
        var i = 0;

        while (i <= step)
        {
            SetPixel((int)MathF.Round(x, MidpointRounding.AwayFromZero), (int)MathF.Round(y, MidpointRounding.AwayFromZero),Color.Red);
            x += dx;
            y += dy;
            i++;
            
        }
    }
    

   
    
    public static void CreateNewImage()
    {
        int width = 800;
        int height = 800;
        Vector3[,] newimage = new Vector3[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float r = 0;
                float g = 0;
                float b = 0;
                newimage[x, y] = new Vector3(r, g, b);
            }
        }
        image = newimage;
    }
    
    public static void CreateNewDepthBuffer()
    {
        int width = 800;
        int height = 800;
        Vector3[,] newDepth = new Vector3[width, height];

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float r = 0;
                float g = 0;
                float b = 0;
                newDepth[x, y] = new Vector3(r, g, b);
            }
        }

        depthBuffer = newDepth;
    }

    public static void SetPixel(int x, int y, Color c )
    {
 
        
        if (x < 0 || y < 0 || x >= image.GetLength(0) || y >= image.GetLength(1))
        {

            return;
        }
    
     
        
        image[x,y] = new Vector3(c.R * 255, c.G * 255, c.B * 255);

    }

    public static byte[] GetImageAsByteArray()
    {
        for (int x = 0; x<800; x++)
        {
            for (int y = 0; y < 800; y++)
            {
                Vector3 col = image[x,799-y];
                int index = x + (y * 800);
                imageBytes[index * 4] = ((byte)(col.X * 255));
                imageBytes[index * 4 + 1] = ((byte)(col.Y * 255));
                imageBytes[index * 4 + 2] = ((byte)(col.Z * 255));
                imageBytes[index * 4 + 3] = (byte)(255);
            }
        }

        return imageBytes;

    }
    
}

