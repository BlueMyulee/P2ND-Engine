using System.Text.Json;
using Engine.Resource;
using Engine_Scenestates;
using static Raylib_cs.Raylib;
using Engine.Game.Objects;
using System.Numerics;
using Raylib_cs;

namespace Engine.Game.Resource.Load.Assets
{

    //IM GONNA CRY || UPDATE: I might not cry.
    public class Material
    {
        public int MaterialId {get; set;}
        public string MaterialName { get; set; }
        public string Shader { get; set; } //MAT_PBR, MAT_FBR, Etc etc...
        public string Albedo { get; set; }
        public string Normal { get; set; }
        public string Mrao { get; set; }
        public float AlbedoIntensity { get; set; }
        public float RoughnessIntensity { get; set; }
        public float MetalnessIntensity { get; set; }
        public float AmbientIntensity { get; set; }
        public float EmissionIntensity { get; set; }
        public bool isLoadedInMemory = false;
        public int memoryLocation = 0;
        public Material(string MaterialName, string Shader, string Albedo, string Normal, string Mrao, float AlbedoIntensity, float RoughnessIntensity, float MetalnessIntensity, float AmbientIntensity, float EmissionIntensity)
        {
            this.MaterialName = MaterialName;
            this.Shader = Shader;
            this.Albedo = Albedo;
            this.Normal = Normal;
            this.Mrao = Mrao;
            this.AlbedoIntensity = AlbedoIntensity;
            this.RoughnessIntensity = RoughnessIntensity;
            this.MetalnessIntensity = MetalnessIntensity;
            this.AmbientIntensity = AmbientIntensity;
            this.EmissionIntensity = EmissionIntensity;
        }

    }

    public class Models
    {
        public string MdlQ { get; set; }
        public string ModelName { get; set; }
        public bool isLoaded = false;
        public int locationInMem;
        
    }

    public class EntityFabs
    {
         public string[] Components { get; set;} 
    }

    public class MapCluster //0x000000 will be pre-requisites., 0x000001 - 0x00000F will be in vertical order, 
    {                       //Subsequently 0x000011 - 0x00001F will move 1 space horizontal and repeat vertical order, rinse and repeat until it becomes 0xFFFFFF.
                            //Also, Mapcluster domains will be 200 x 200 x 200
        public string mapName { get; set; }
        public int[] entityRef { get; set; }
        public float[] entTransformX {get; set;}
        public float[] entTransformY {get; set;}
        public float[] entTransformZ {get; set;}
        public int XLeftAdjecent { get; set; } 
        public int XRightAdjecent { get; set; } 
        public int YUpAdjecent { get; set; } 
        public int YDownAdjecent { get; set; } 
    }
    public class LoadQueueMapProp
    {
        public int MdlId { get; set; }
        public int PRefId { get; set; }
        public Prop_types Prptyp { get; set; }
        public string PrNameId { get; set; }
        public bool IsInteractable { get; set; }
        public int InteractionId{ get; set; }
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public float Scale { get; set; }

        public LoadQueueMapProp(int mid, int prid, Prop_types ptype, string pnid, bool intrb, int inid, Vector3 pos, Vector3 rot, float scl)
        {
            MdlId = mid;
            PRefId = prid;
            Prptyp = ptype;
            PrNameId = pnid;
            IsInteractable = intrb;
            InteractionId = inid;
            Position = pos;
            Rotation = rot;
            Scale = scl;
        }
    }

    // [DEPRECATED CODE] ALL THE CODE BELOW ARE NOW REDUNDANT!
    /* class LoadingThreads
    {
        public static int CurrentLoad;
        public static List<LoadQueueResourceMdl> MdlQueue = new();
        public static List<LoadQueueMapProp> MapPropQueue = new();
        static void Loading(int i)
        {
            CurrentLoad = i;
            Thread loadSubroutine = new Thread(LoadSubroutine);
            
        }
        //Loading stuff though maybe I should do this asynchronously.

        private static void LoadSubroutine()
        {
            //Thinking about renaming the extension to .
            string cacheDir = Directory.GetCurrentDirectory() + @"\resources\req" + $@"\m_cache_part_{CurrentLoad}.fwwvch"; //fuck wrong with val?!
            string mapDir = Directory.GetCurrentDirectory() + @"\resources\map" + $@"\map_sect_{CurrentLoad}.fwwvmp";
            string colDir = Directory.GetCurrentDirectory() + @"\resources\map" + $@"\col_info_{CurrentLoad}.fwwvcl";
            string matDir = Directory.GetCurrentDirectory() + @"\resources\req" + $@"\mat_cache_part_{CurrentLoad}.fwwvmt";  // Embedding the material INTO the model seems more favorable
            string loaded = File.ReadAllText(cacheDir);                                                                      //Time saving wise, but it seems inefficient for any models reusing a material.
            string loadedmp = File.ReadAllText(mapDir);                                                                      //idrc so ehh.
            MdlQueue.Clear();
            MapPropQueue.Clear();
            MdlQueue = JsonSerializer.Deserialize<List<LoadQueueResourceMdl>>(loaded);
            MapPropQueue = JsonSerializer.Deserialize<List<LoadQueueMapProp>>(loadedmp);


            //TODO: Resource caching loop 
            //1. Load the Material file first I think. save them all in the list.
            //2. Load and run a loop that applies the model's texture materials via a crude search I++

            //Everything here is subject for re-write.
            /*for (int i = 0; i < MdlQueue.Count - 1; i++)
            {
                Scenestate.modl.Add(new Mdl(LoadModel(MdlQueue[i].MdlQ), MdlQueue[i].Objectid, MdlQueue[i].Itemid));
            }

            for (int i = 0; i < MapPropQueue.Count - 1; i++)
            {

                for (int n = 0; n < Scenestate.modl.Count - 1; n++)
                {
                    Model submdls;

                    if (Scenestate.modl[n].ObjId == MapPropQueue[i].MdlId)
                    {
                        submdls = Scenestate.modl[n].mdl;
                        Scenestate.prop.Add(new GameObjects.Props(submdls, MapPropQueue[i].PRefId, MapPropQueue[i].Prptyp, MapPropQueue[i].PrNameId, MapPropQueue[i].IsInteractable, MapPropQueue[i].InteractionId, MapPropQueue[i].Position, MapPropQueue[i].Rotation, MapPropQueue[i].Scale));
                        break;//I feel like eating crawfish
                    }

                }
            }

        }
    }*/
}