
using System.Numerics;
using Engine.Game.Resource.Load.Assets;
using Engine.Game.Resource.Load.IO;
using ComponentSystem;
namespace Engine.scenestate
{
    class Entity
    {
        public static void LoadMapClusterData(int mapClusterLocation)
        {
            var mapclust = Game.Resource.Load.IO.Assets.mapCl;
            
            Console.WriteLine("\n\n\n\n GRINKLE1");
            for(int n = 0; n < mapclust[mapClusterLocation].entityRef.Length; n++)
            {
                
                Console.WriteLine($"\n\n\n\n {mapclust.Count}");
                SetupEntitiesInCluster(mapclust[mapClusterLocation].entityRef[n], new Vector3(mapclust[mapClusterLocation].entTransformX[n], mapclust[mapClusterLocation].entTransformY[n], mapclust[mapClusterLocation].entTransformZ[n]), 1.0f);

            }
            
        }

        public static void SetupEntitiesInCluster(int addr, Vector3 transf, float scale)
        {
            
            Engine_Scenestates.Scenestate.entities.Add(new Game.Entity());
            Console.WriteLine($"\n\n DEBUG: ATTEMPING TO FIND pass\n\n\n\n");
            int curcount = Engine_Scenestates.Scenestate.entities.Count - 1;

            var entloc = Assets.entFab;

            Console.WriteLine($"\n\n DEBUG: ATTEMPING TO FIND {entloc[0].Components.Length}\n\n\n\n");

            for(int i = 0; i < entloc[addr].Components.Length; i++) // Specifically this
            {
                Engine_Scenestates.Scenestate.entities[curcount].AddComponent(new Transform());
                Engine_Scenestates.Scenestate.entities[curcount].GetComponent<Transform>().position = transf;
                Engine_Scenestates.Scenestate.entities[curcount].GetComponent<Transform>().scale.X = scale;

                if(entloc[addr].Components[i].Contains(':'))
                {
                    string[] tags = entloc[addr].Components[i].Split(':', ' ');
                    int[] reference = {};
                    
                    for(int n = 1 ; n < tags.Length; n++)
                    {
                        reference = reference.Append(1).ToArray();
                        reference[reference.Length - 1] = Convert.ToInt32(tags[n]);
                    }
                    switch(tags[0])
                    {
                        case "mesh":   
                            Engine_Scenestates.Scenestate.entities[curcount].AddComponent(new Mesh3D());
                            Engine_Scenestates.Scenestate.entities[curcount].GetComponent<Mesh3D>().meshAssigned = reference[0];
                        break;

                        case "mats":
                            for(int w = 0; w < reference.Length; w++)
                            {
                                Engine_Scenestates.Scenestate.entities[curcount].GetComponent<Mesh3D>().materialAssigned = Engine_Scenestates.Scenestate.entities[curcount].GetComponent<Mesh3D>().materialAssigned.Append(1).ToArray();
                                Engine_Scenestates.Scenestate.entities[curcount].GetComponent<Mesh3D>().materialAssigned[w] = reference[w];
                            }

                        break;
                        case "lightdir":
                            Engine_Scenestates.Scenestate.entities[curcount].AddComponent(new LightDir());
                            var lightdirops = Engine_Scenestates.Scenestate.entities[curcount].GetComponent<LightDir>();
                            lightdirops.target.X = reference[0];
                            lightdirops.target.Y = reference[1];
                            lightdirops.target.Z = reference[2];
                            lightdirops.intensity = reference[3];
                        break;
                        case "lcolor":
                        break;
                    }
                }
                else
                {

                    switch(entloc[addr].Components[i])
                    {
                        case "skybox":
                        Engine_Scenestates.Scenestate.entities[curcount].AddComponent(new Skybox());
                        break;
                    }


                }

            }
        }

    }
}