using static Raylib_cs.Raylib;
using Engine.Game.Objects;
using Engine.Logics.Sub.PlayerCon.FirstPerson;
using Engine_Scenestates;
using Raylib_cs;
using Engine.Resource;
using Engine.Game;
using System.Numerics;
using Engine.Render.LightingEngine;
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
            BeginMode3D(ControlCorrespondant.camfps);
             //   DrawGrid(40, 5);
                //DrawCube(new Vector3(0, 0, 100), 1, 1, 1, Color.White);
                //DrawCube(new Vector3(0, 0, -100), 1, 1, 1, Color.White);
                //DrawCube(new Vector3(0, 100, 0), 1, 1, 1, Color.White);
               // DrawCube(new Vector3(0, 1, 0), 1, 1, 1, Color.White);
                RenderBrush();
                //SetShaderValueMatrix();
                //DrawModel(Scenestate.entities[0].Getcomponent<Mesh3D>().model, Vector3.Zero, 1, Raylib_cs.Color.White);
                TransformSystem.Update(GetFrameTime());
                Mesh3DSystem.Update(GetFrameTime());
                //Rlgl.DisableDepthMask();
                Mesh3DTransSystem.Update(GetFrameTime());
                //Rlgl.EnableDepthMask();
                EndMode3D();  
            }
            
        }

        private class Rend_UI
        {
            public static void Render_FpsCounter()
            {
                DrawText($"Current fps: {GetFPS()}", 10, 10, 80, Raylib_cs.Color.SkyBlue);
                DrawText($"Mem: {Engine.Logics.Dev.Variables.memory:F2}MB Actual: {Engine.Logics.Dev.Variables.memoryActual:F2}MB || Screenwidth : {GetScreenWidth()} x {GetScreenHeight()}", 10, 120, 50, Raylib_cs.Color.SkyBlue);
                //BeginBlendMode(BlendMode.Multiplied);
                //DrawRectangle(0, 0, 2900, 2900, new Raylib_cs.Color(00.957f, 0.851f, 0.20f, 0.07f));
                //BeginBlendMode(BlendMode.Additive);
                //DrawRectangle(0, 0, 2900, 2900, new Raylib_cs.Color(0.255f, 0.380f, 0.184f, 0.1f));
                //EndBlendMode();
                //EndBlendMode();
            }
        }

        public static unsafe void Rend_Unified()
        {
            Lighting.RenderLightPass();
            UpdateCamera(ref ControlCorrespondant.camfps, CameraMode.Custom);          
            BeginDrawing();
            ClearBackground(Raylib_cs.Color.Black);
            SetShaderValueMatrix(Shadercl.Mat_PBR, Shadercl.lightVPLoc, Lighting.lightViewProj);
            Rlgl.EnableShader(Shadercl.Mat_PBR.Id);
            Rlgl.ActiveTextureSlot(Lighting.textureActiveSlot);
            Rlgl.EnableTexture(Lighting.shadowMap.Depth.Id);
            var tl = Lighting.textureActiveSlot;
            Rlgl.SetUniform(Shadercl.shadowMapLoc, &tl, (int)ShaderUniformDataType.Int, 1);
            SetShaderValueTexture(Shadercl.Mat_PBR, Shadercl.shadowMapLoc, Lighting.shadowMap.Depth);
            Rlgl.EnableDepthMask();
            BeginBlendMode(BlendMode.Alpha);//TODO: figure out why the hell transparency keeps clipping everything behind it.
            //BeginShaderMode(Scenestate.yella);
            Rend_3D.Render_3D();
            Rlgl.ActiveTextureSlot(Lighting.textureActiveSlot);
            Rlgl.DisableTexture();
            EndBlendMode();
            EndShaderMode();
            Rend_UI.Render_FpsCounter();
            EndDrawing();
        

        }
    }

}
