using ComponentSystem;
using static Raylib_cs.Raylib;
using Engine_Scenestates;
using Raylib_cs;
using Engine.Resource;
namespace Engine.Render.MaterialSystem
{
    public class MaterialsQueue
    {
        public static int preloadedCheck = 0x000000;
        public int ActiveMaterials = 0;
        public Texture2D albedo;
        public Shader shader;
        public Texture2D normal;
        public Texture2D mrao;
        public float AlbedoIntensity;
        public float RoughnessIntensity;
        public float AmbientIntensity;
        public float EmissionIntensity;
        public string test;

        public unsafe MaterialsQueue(string Alb, string norm, string mra, string shadr, float aInt, float rouInt, float ambInt, string testcheck)
        {
        


            

            if(shadr == "PBR")
            {
                this.shader = Shadercl.Mat_PBR;
                Texture2D albed = LoadTexture(Alb);
                Texture2D normed = LoadTexture(norm);
                Texture2D mrted = LoadTexture(mra);            
                GenTextureMipmaps(&albed);
                GenTextureMipmaps(&normed);
                GenTextureMipmaps(&mrted);
                this.albedo = albed;
                this.normal = normed;
                this.mrao = mrted;


             SetTextureFilter(this.albedo, TextureFilter.Bilinear);
            
                SetTextureFilter(this.normal, TextureFilter.Bilinear);
                SetTextureFilter(this.normal, TextureFilter.Anisotropic8X);

                SetTextureFilter(this.mrao, TextureFilter.Bilinear);
                SetTextureFilter(this.mrao, TextureFilter.Anisotropic8X);
            }
            else
            {
                Texture2D albed = LoadTexture(Alb);
                SetTextureFilter(this.albedo, TextureFilter.Bilinear);
            }

            test = testcheck;
        }
    }

    public class MaterialMemorySystem
    {
        public static List<MaterialsQueue> materials = new();
        public static unsafe void checkActiveMaterialInScene()
        {
            for(int i = 0; i < Scenestate.entities.Count; i++)
            {
                if(Scenestate.entities[i].HasComponent<Mesh3DTrans>() == true)
                continue;

                var materialref = Scenestate.entities[i].GetComponent<Mesh3D>();
                if(materialref == null || materialref.materialAssigned == null)
                continue;
                
                if(materialref.matAllLoaded == true)
                continue;

                for(int n = 0; n < materialref.materialAssigned.Length; n++)
                {
                    if(materialref.materialAssignedInMemory.Length == 0)
                    {
                        Console.WriteLine("\n" + $"|| INFO: MATERIAL: CurrentVal: {materialref.materialAssigned[n]}\n\n\n\n\n\n");
                        var Loadqueue = Engine.Game.Resource.Load.IO.Assets.mat[materialref.materialAssigned[n]];
                        materials.AddRange(new MaterialsQueue(Loadqueue.Albedo, Loadqueue.Normal, Loadqueue.Mrao, Loadqueue.Shader, Loadqueue.AlbedoIntensity, Loadqueue.RoughnessIntensity, Loadqueue.AmbientIntensity, "checking"));
                        if(Loadqueue.Shader != "PBR") 
                        {
                            SetTextureFilter(materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Albedo].Texture, TextureFilter.Bilinear);
                            continue;
                        }
                        materialref.materialAssignedInMemory = materialref.materialAssignedInMemory.Append(1).ToArray();                        
                        materialref.materialAssignedInMemory[n] = materials.Count - 1;
                        Console.WriteLine("\n\n" + $"|| INFO: MATERIAL: CurrentCount: {materialref.model.MaterialCount} {n + 1} {materials.Count} {materialref.materialAssignedInMemory.Length} {materials[0].test}\n\n\n\n\n\n");
                        materialref.model.Materials[n + 1].Shader = materials[0].shader;
                        materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Albedo].Color = Raylib_cs.Color.White;
                        materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Albedo].Texture = materials[materialref.materialAssignedInMemory[n]].albedo;
                        materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Normal].Texture = materials[materialref.materialAssignedInMemory[n]].normal;
                        materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Metalness].Texture = materials[materialref.materialAssignedInMemory[n]].mrao;
                        Console.WriteLine("||INFO MATERIAL: Material Load Success");
                        materialref.matAllLoaded = true;
                        continue;

                    }
                    else
                    {
                     Console.WriteLine("||INFO MATERIAL: Material already exists");
                        materialref.model.Materials[n + 1].Shader = materials[materialref.materialAssignedInMemory[n]].shader;
                        materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Albedo].Texture = materials[materialref.materialAssignedInMemory[n]].albedo;
                        materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Normal].Texture = materials[materialref.materialAssignedInMemory[n]].normal;
                        materialref.model.Materials[n + 1].Maps[(int)MaterialMapIndex.Metalness].Texture = materials[materialref.materialAssignedInMemory[n]].mrao;
                        materialref.matAllLoaded = true;
                        
                    }
                }
            }
        }        
    }
     
}