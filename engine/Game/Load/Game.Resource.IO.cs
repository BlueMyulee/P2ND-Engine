using System.Text.Json;
using static Raylib_cs.Raylib;
using Engine.Game.Objects;
using System.Numerics;
using Raylib_cs;
using Engine.Game.Resource.Load;


namespace Engine.Game.Resource.Load.IO
{
    public class Assets
    {
        public static List<Load.Assets.Material> mat = new();
        public static List<Load.Assets.Models> models = new();
        public static List<Load.Assets.MapCluster> mapCl = new();
        public static List<Load.Assets.EntityFabs> entFab = new();

        public static void LoadMaterials()
        {
            int dirFileCount = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "/resources/materials/", "*.p2mat").Count();
            for(int i = 0; i < dirFileCount; i++)
            {
                string readfile = File.ReadAllText(Directory.GetCurrentDirectory() + $"/resources/materials/{i.ToString("x6")}.p2mat");
                Console.WriteLine("\n|| MATERIAL: " + readfile);
                var matload = JsonSerializer.Deserialize<Game.Resource.Load.Assets.Material>(readfile);

                mat.AddRange(matload);

                Console.WriteLine("0x" + mat[0x000000].MaterialId.ToString("x6"));
            }
            Console.WriteLine("\n|| INFO: MATERIALS: All addresses loaded successfully");
        }

        public static void LoadModels()
        {
            int dirFileCount = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "/resources/models/", "*.p2mdl").Count();
            for(int i = 0; i < dirFileCount; i++)
            {
                string readfile = File.ReadAllText(Directory.GetCurrentDirectory() + $"/resources/models/{i.ToString("x6")}.p2mdl");
                Console.WriteLine("\n|| MODEL: " + readfile);
                var modload = JsonSerializer.Deserialize<Game.Resource.Load.Assets.Models>(readfile);

                models.AddRange(modload);
            } 
            Console.WriteLine("\n|| INFO: MODELS: All addresses loaded successfully");
        }

        public static void LoadMapClusters()//NOTE, Mapclusters are 200x200 
        {
            int dirFileCount = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "/resources/mapclusters/", "*.p2mapcl").Count();
            for(int i = 0; i < dirFileCount; i++)
            {
                string readfile = File.ReadAllText(Directory.GetCurrentDirectory() + $"/resources/mapclusters/{i.ToString("x6")}.p2mapcl");
                Console.WriteLine("\n|| MAPCLUSTER: " + readfile);
                var mapload = JsonSerializer.Deserialize<Game.Resource.Load.Assets.MapCluster>(readfile);

                mapCl.Add(mapload);
            }
            Console.WriteLine("\n|| INFO: MAPCLUSTER: All addresses loaded successfully");
        }

        public static void LoadEntity() 
        {
            int dirFileCount = Directory.EnumerateFiles(Directory.GetCurrentDirectory() + "/resources/entity/", "*.p2ent").Count();
            for(int i = 0; i < dirFileCount; i++)
            {
                string readfile = File.ReadAllText(Directory.GetCurrentDirectory() + $"/resources/entity/{i.ToString("x6")}.p2ent");
                Console.WriteLine("\n|| ENT: " + readfile);
                var entload = JsonSerializer.Deserialize<Game.Resource.Load.Assets.EntityFabs>(readfile);

                entFab.Add(entload);
            }
        }
        
        
    }
}
