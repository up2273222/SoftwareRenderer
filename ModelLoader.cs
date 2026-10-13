using System.Numerics;

namespace SoftwareRendererCS;

public class ModelLoader
{
   
    public static Model LoadOBJ(string path)
    {
        
      
        using StreamReader objReader = new StreamReader(path);
        string[] lines = objReader.ReadToEnd().Split('\n');
        
        List<Vector3> vertices = new List<Vector3>();
        List<int> indices = new List<int>();
        //v 6.023572 -24.746058 3.395914
        //f 1/101/1 2/107/2 8/57/3 7/58/4
        
        for (int i = 0; i < lines.Length; i++)
        {
            // Console.WriteLine(lines[i]);
            string currentLine = lines[i];
            if (currentLine.StartsWith("v "))
            {
                string[] vectorSplit = currentLine.Split(" ");
                Vector3 vector = new Vector3(float.Parse(vectorSplit[1]), float.Parse(vectorSplit[2]), float.Parse(vectorSplit[3]));
                vertices.Add(vector);
            }
            if (currentLine.StartsWith("f "))
            {
                string[] faceSplit = currentLine.Split(" ");
                for (int j = 1; j <= 3; j++)
                {
                    string line = faceSplit[j];
                    string face = "";
                
                    for (int x = 0; x < line.Length; x++)
                    {
                        char c = line[x];
                        if (c != '/')
                        {
                            face = face.Insert(x,c.ToString());
                        }
                        else
                        {
                            indices.Add(int.Parse(face)-1);
                            break;
                        }
                    }
               

                }
          

            }
        }
        objReader.Close();
            
        Model outModel = new Model();
        outModel.vertices = vertices;
        outModel.indices = indices;
        return outModel;
        

       
    }
}