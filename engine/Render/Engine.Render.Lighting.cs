using System.Numerics;
using Engine.Game.Objects;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Engine.Game;
using Engine.Resource;
using Engine_Scenestates;
using Engine.Logics.Sub.PlayerCon.FirstPerson;
using ComponentSystem;
namespace Engine.Render.LightingEngine
{
        public class Lighting
        {
            public static Camera3D lCam = new();
            public static int lightCount = 0;
            public static int[] lightAddress = { };
            public static RenderTexture2D shadowMap = Shadercl.LoadShadowmapRenderTexture(1024, 1024);
            public static Matrix4x4 lightView = new();
            public static Matrix4x4 lightProj = new();
            public static Matrix4x4 lightViewProj = new();
            public static int textureActiveSlot = 10;

            public static void UpdateLight(Shader shad, GameObjects.Lights lights)
            {

                SetShaderValue(shad, lights.enabledLoc, lights.Enabled, ShaderUniformDataType.Int);
                SetShaderValue(shad, lights.typeLoc, lights.Type, ShaderUniformDataType.Int);

                SetShaderValue(shad, lights.positionLoc, lights.Position, ShaderUniformDataType.Vec3);
                
                SetShaderValue(shad,lights.targetLoc, lights.Target, ShaderUniformDataType.Vec3);
                SetShaderValue(shad, lights.colorLoc, lights.Colour, ShaderUniformDataType.Vec4);
                SetShaderValue(shad, lights.intensityLoc, lights.Intensity, ShaderUniformDataType.Float);
            }

            public static void SetupLCam()
            {
                lCam.Position = Vector3.Add(ControlCorrespondant.camfps.Position, new Vector3(400f, 400f, 400f));
                lCam.Target = ControlCorrespondant.camfps.Position;
                lCam.Projection = CameraProjection.Orthographic;
                lCam.Up = new Vector3(0.0f, 1.0f, 0.0f);
                lCam.FovY = 20.0f;
            }


            public static unsafe void RenderLightPass()
            {
                Vector3 campos = ControlCorrespondant.camfps.Position;
                SetShaderValue(Shadercl.Mat_PBR, Shadercl.Mat_PBR.Locs[(int)ShaderLocationIndex.VectorView], &campos, ShaderUniformDataType.Vec3);
                //for(int i = 0; i < lightAddress.Length; i++)
                //{
                    int i = 0;
                    Console.WriteLine("Current pass: " + i);
                    var _TempTarg = Scenestate.entities[lightAddress[i]].GetComponent<LightDir>().target;
                    var _TempPos = Scenestate.entities[lightAddress[i]].GetComponent<ComponentSystem.Transform>().position;
                    Vector3 lightDirection = Vector3.Normalize(_TempPos - _TempTarg);
                    lCam.Target = _TempTarg;
                    lCam.Position = _TempTarg + lightDirection * 15.0f;
                    BeginTextureMode(shadowMap);
                    if(i == 0)
                    ClearBackground(Raylib_cs.Color.White);
                    BeginMode3D(lCam);
                    BeginShaderMode(Shadercl.Shadow);
                    lightView = Rlgl.GetMatrixModelview();
                    lightProj = Rlgl.GetMatrixProjection();
                    
                    Mesh3DSystem.Update(GetFrameTime());
                    EndShaderMode();
                    EndMode3D();
                    //Rlgl.Enable
                    EndTextureMode();

                    if(i == 0)
                    lightViewProj = lightView * lightProj;
                //}
            }
    }

}