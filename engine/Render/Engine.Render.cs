using static Raylib_cs.Raylib;
using Engine.Game.Objects;
using Engine.Logics.Sub.PlayerCon.FirstPerson;
using Engine_Scenestates;
using Raylib_cs;
using Engine.Resource;
using Engine.Game;
using System.Numerics;
using ComponentSystem;

namespace Engine.Render
{
    public class Renders
    {
        private class Rend_2D
        {

        }

        private static class Rend_3D
        {
            public static void RenderBrush()
            {
                    Shadercl.ShaderUpdateRuntimeDuringPBR();
            }

            public static void Render_3D()
            {
                Lighting.UpdateLight(Shadercl.Mat_PBR, Scenestate.ligt[0]);
                Lighting.UpdateLight(Shadercl.Mat_PBR, Scenestate.ligt[1]);
                //Console.WriteLine(Scenestate.ligt[0].Position);
                BeginMode3D(ControlCorrespondant.camfps);
                DrawGrid(40, 5);
                DrawCube(new Vector3(0, 0, 100), 1, 1, 1, Color.White);
                DrawCube(new Vector3(0, 0, -100), 1, 1, 1, Color.White);
                DrawCube(new Vector3(0, 100, 0), 1, 1, 1, Color.White);
                DrawCube(new Vector3(0, -100, 0), 1, 1, 1, Color.White);
                RenderBrush();
                //DrawModel(Scenestate.entities[0].Getcomponent<Mesh3D>().model, Vector3.Zero, 1, Raylib_cs.Color.White);
                TransformSystem.Update(GetFrameTime());
                Mesh3DSystem.Update(GetFrameTime());
                Rlgl.DisableDepthMask();
                Mesh3DTransSystem.Update(GetFrameTime());
                Rlgl.EnableDepthMask();
                EndMode3D();  
            }
            
        }

        private class Rend_UI
        {
            public static void Render_FpsCounter()
            {
                DrawText($"Current fps: {GetFPS()}", 10, 10, 80, Raylib_cs.Color.DarkBlue);
                DrawText($"Mem: {Engine.Logics.Dev.Variables.memory:F2}MB Actual: {Engine.Logics.Dev.Variables.memoryActual:F2}MB || Screenwidth : {GetScreenWidth()} x {GetScreenHeight()}", 10, 120, 50, Raylib_cs.Color.DarkBlue);
                //BeginBlendMode(BlendMode.Multiplied);
                //DrawRectangle(0, 0, 2900, 2900, new Raylib_cs.Color(00.957f, 0.851f, 0.20f, 0.07f));
                //BeginBlendMode(BlendMode.Additive);
                //DrawRectangle(0, 0, 2900, 2900, new Raylib_cs.Color(0.255f, 0.380f, 0.184f, 0.1f));
                //EndBlendMode();
                //EndBlendMode();
            }
        }

        public static void Rend_Unified()
        {
            UpdateCamera(ref ControlCorrespondant.camfps, CameraMode.Custom);
            BeginDrawing();

            BeginBlendMode(BlendMode.Alpha);//TODO: figure out why the hell transparency keeps clipping everything behind it.
            ClearBackground(Raylib_cs.Color.Black);
            BeginShaderMode(Scenestate.yella);
            Rend_3D.Render_3D();
            EndBlendMode();
            EndShaderMode();
            Rend_UI.Render_FpsCounter();
            EndDrawing();
        

        }
    }

    public class Lighting
    {
        public static void UpdateLight(Shader shad, GameObjects.Lights lige)
        {

                SetShaderValue(shad, lige.enabledLoc, lige.Enabled, ShaderUniformDataType.Int);
                SetShaderValue(shad, lige.typeLoc, lige.Type, ShaderUniformDataType.Int);


                SetShaderValue(shad, lige.positionLoc, lige.Position, ShaderUniformDataType.Vec3);
                
                SetShaderValue(shad,lige.targetLoc, lige.Target, ShaderUniformDataType.Vec3);
                SetShaderValue(shad, lige.colorLoc, lige.Colour, ShaderUniformDataType.Vec4);
                SetShaderValue(shad, lige.intensityLoc, lige.Intensity, ShaderUniformDataType.Float);
        }
    }
}
