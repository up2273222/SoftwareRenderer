using System.Diagnostics;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Runtime.Intrinsics.X86;
using Raylib_cs;
using static SoftwareRendererCS.MathLib;
using static SoftwareRendererCS.ImageCreator;
using static SoftwareRendererCS.ModelLoader;
using Random = System.Random;

namespace SoftwareRendererCS;

public class Program
{

   
    public static void Main(string[] args)
    {
        CreateNewImage();
        InitByteArray();
        
        List<Model> modelsInScene = new List<Model>();
        Model model = new Model();
        
        int dimension = 1;
        
        Matrix4x4 ortho = CreateOrthographicProjectionMatrix(-dimension,dimension,-dimension,dimension,-100,100);

        model = LoadOBJ("C:/Users/archi/Documents/SoftwareRenderer/SoftwareRenderer/SoftwareRenderer/diablo3_pose.obj");
        modelsInScene.Add(model);
        
       
        
       
   
        
        Raylib.InitWindow(800, 800, "Window");
        Texture2D tex = Raylib.LoadTextureFromImage(Raylib.GenImageColor(800,800,Color.Black));
        
        byte[] texBytes = new byte[tex.Width * tex.Height * 4];

        int frame = 0;

        while (!Raylib.WindowShouldClose())
        {
            foreach (Model modelInScene in modelsInScene)
            {
                for (int i = 0; i < modelInScene.indices.Count; i +=3 )
                {
                    Triangle triangle = new Triangle(
                        modelInScene.vertices[modelInScene.indices[i]],
                        modelInScene.vertices[modelInScene.indices[i + 1]],
                        modelInScene.vertices[modelInScene.indices[i + 2]]);

                    triangle.p1 = RotateAroundAxis(triangle.p1, new Vector3(0, 1, 0), frame);
                    triangle.p2 = RotateAroundAxis(triangle.p2, new Vector3(0, 1, 0), frame);
                    triangle.p3 = RotateAroundAxis(triangle.p3, new Vector3(0, 1, 0), frame);

                    triangle.p1 = TransformPoint(ortho, triangle.p1);
                    triangle.p2 = TransformPoint(ortho, triangle.p2);
                    triangle.p3 = TransformPoint(ortho, triangle.p3);
                    
          
                   // DrawDDATriangle(triangle); 
                    FillTriangle(triangle);
                }
            }
           
        
        
                texBytes = GetImageAsByteArray();
                Raylib.UpdateTexture(tex,texBytes);
                Raylib.BeginDrawing();
                Raylib.DrawTexture(tex,0,0,Color.White);
                Raylib.EndDrawing();
                ClearFrame();
                frame++;
                Console.WriteLine(Raylib.GetFPS());

        }
        Raylib.CloseWindow();

    
      

      //  WriteImageToFile();
   
    }


   

 
    
    
    

   
}

    

  
    
   



