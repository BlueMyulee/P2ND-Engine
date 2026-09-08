using System.Numerics;
using System;
using System.Linq;
using System.Text;
using static Raylib_cs.Raylib;
using System.Drawing;
using Engine.Game.Objects;
using Engine.Resource;
using Engine.Render.MaterialSystem;
using Raylib_cs;
using Engine.Logics.Sub.PlayerCon.FirstPerson;
using Engine.Game;
using ComponentSystem;
using Engine.Render.MeshSystem;


namespace Engine_Scenestates
{
    class Scenestate
    {
        public static List<Engine.Game.Objects.GameObjects.Lights> ligt = new List<GameObjects.Lights>();
        public static Shader yella;
        public static RenderTexture2D target;
        public static List<Entity> entities = new();

        private static int state_id = 0; // 0 = splashscreen, 1 = menu, 2 = loading thread, 3 = ingame, 4 = test ingame[WILL DEPRECATE SOON]
        public class States
        {

            public static void SwitcScene(int sc)
            {
                state_id = sc;
                bool loadcycle = true;
                
                if (loadcycle == true)
                    Maps.LoadAll();
                
                loadcycle = false;
            }
        }

        public class Maps // 0 = testmap, 1 = nmap
        {

            public static void LoadAll()
            {
                Shadercl.InitializeShaderPBR();
                LoadPrereq();
                if (state_id == 4)
                { 
                    ligt.Add(new GameObjects.Lights(new Vector3(100f, 0, 0f), new Vector3(0f, 0.0f, 0.0f), false, Light_types.Point, Raylib_cs.Color.White, 0.0f, Shadercl.Mat_PBR));  
                    //new Vector3(5.5f, -1.0f, 5.0f)
                    ligt.Add(new GameObjects.Lights(new Vector3(1f, 2f, 1f), new Vector3(10.5f, 0.0f, 8.0f), true, Light_types.Directorional, Raylib_cs.Color.White, 20.0f, Shadercl.Mat_PBR));
                    //ligt.Add(new GameObjects.Lights(new Vector3(1f, 0f, 1f), new Vector3(10.5f, 1.0f, 4.0f), true, Light_types.Point, Raylib_cs.Color.RayWhite, 0.0f, Shadercl.Mat_PBR));  at_PBR));  
                    ligt.Add(new GameObjects.Lights(new Vector3(0f, 5f, -10f), new Vector3(-10f, 0f, 8.0f), true, Light_types.Directorional, Raylib_cs.Color.Violet, 20.0f, Shadercl.Mat_PBR));  
                    //Engine.Game.Objects.GameObjects.Lights.UpdateShaderValues(ligt[0], Shadercl.Mat_PBR);
                    Engine.Game.Objects.GameObjects.Lights.UpdateShaderValues(ligt[2], Shadercl.Mat_PBR);
                    Engine.Game.Objects.GameObjects.Lights.UpdateShaderValues(ligt[1], Shadercl.Mat_PBR);
                    //prop.Add(new GameObjects.Props(modl[0].mdl, 0x000002, Prop_types.Prop_Static, "gag", false, 0x000000, new Vector3(0.0f, 0f, 0.0f), Vector3.Zero, 1.0f));
                    ControlCorrespondant.SetGameDefault(new Vector3(0.0f, 0.0f, 0.0f));
                    Console.WriteLine("DONE______________________________________________________________!\n");
                    
                }
            }

            public static void LoadPrereq()
            {
                target = LoadRenderTexture(GetScreenWidth(), GetScreenHeight());
                yella = LoadShader(null, "resources/shader/screen/yellowfilter.fs");
                entities.Add(new Entity());
                ComponentSystem.Transform transform = new ComponentSystem.Transform();
                transform.position = new Vector3(0, 0, 0);
                transform.scale.X = 1;
                
                ComponentSystem.Mesh3D mesh = new Mesh3D();
                mesh.meshAssigned = 0;
                mesh.materialAssigned = mesh.materialAssigned.Append(1).ToArray();
                mesh.materialAssigned[0] = 0;
                entities[0].AddComponent(transform);
                entities[0].AddComponent(mesh);
                entities[0].Id = entities.Count - 1;
                MeshMemorySystem.CheckActiveMeshesInScene();
                MaterialMemorySystem.checkActiveMaterialInScene();
                
                                    


               // GC.Collect();

            }
        
        }

        public static int StateId() { return state_id; }
    }
}
