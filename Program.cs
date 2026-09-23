using System.Diagnostics;
using static Raylib_cs.Raylib;
using Engine_Scenestates;
using Engine.Render;
using Engine.Resource;
using Raylib_cs;
using Engine.Logics.Sub.PlayerCon.FirstPerson;
using Engine.Render.Optimization.Culling;
using Engine.Game;

/****************************************
P2Engine

Changed team name from CSoft to WFIO

Part of a project being made by WFIO With Codename:
P2D Radiant.

Rm9yIHRoZSBnbG9yeSBvZiBKZXN1cyBDaHJpc3Q=

****************************************/
namespace RDN
{
    class Program
    {
        
        public static int Main()
        {
            // Initialization
            //--------------------------------------------------------------------------------------
           

            const int screenWidth = 2320;
            const int screenHeight = 1380;
	        GpuDetection.RunDetection();

            InitWindow(screenWidth, screenHeight, "P2ND Engine Test");
            //SetConfigFlags(Raylib_cs.ConfigFlags.FullscreenMode);
            //Raylib.ToggleBorderlessWindowed();
            Engine.Game.Resource.Load.IO.Assets.LoadModels();
            Engine.Game.Resource.Load.IO.Assets.LoadMaterials();
            Engine.Game.Resource.Load.IO.Assets.LoadMapClusters();
            Engine.Game.Resource.Load.IO.Assets.LoadEntity();
            Scenestate.States.SwitcScene(4);
            Rlgl.EnableDepthTest();
            SetTargetFPS(200);
                while (!WindowShouldClose())
                {
                    Process currentProcess = Process.GetCurrentProcess();
                    ControlCorrespondant.UpdatePlayerLogic();
                    SkyboxSystem.Update(GetFrameTime());
                    Shadercl.ShaderUpdateRuntimePrePBR();    
                    Renders.Rend_Unified();
                    long privateMemoryBytes = currentProcess.PrivateMemorySize64;
                    double privateMemoryMB = privateMemoryBytes / 1024.0 / 1024.0;
                    long actualMemoryBytes = currentProcess.WorkingSet64;
                    double aMemB = actualMemoryBytes / 1024.0 / 1024.0;
                    //Console.WriteLine($"Current RAM usage: {privateMemoryMB:F2} MB");
                    Engine.Logics.Dev.Variables.memory = privateMemoryMB;
                    Engine.Logics.Dev.Variables.memoryActual = aMemB;
                }
            CloseWindow();

            return 0;
    }
    }
}
