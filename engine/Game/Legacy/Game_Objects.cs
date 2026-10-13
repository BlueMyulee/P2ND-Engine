using System.Numerics;
using System;
using System.Linq;
using System.Text;
using static Raylib_cs.Raylib;
using System.Drawing;
using Raylib_cs;
/*************************
Game Objects.

Will be replaced by ECS
--------------------------------
**************************/
namespace Engine.Game.Objects
{
    public enum Prop_types
    {
        Prop_Static,
        Prop_Phys,
        Npc_body,
        Npc_Item,
        Player_Item,
        Player_ViewMdl
    }

    public enum Light_types
    {
        Directorional = 0, //Maybe we should rename it to the SUN hehe
        Point = 1,
        Spot = 2
    }
    public class GameObjects
    {
        public Camera3D lightCam;

        public class Brush // Will not use this other than testing purpose, otherwise will clone instance to make collision detection maybe
        {
            public Vector3 brushScale;
            public Vector3 brushPos;
            public Texture2D brushtex;

        }

        public class Lights
        {
            public Vector3 Position;
            public Vector3 Target;
            public bool Enabled;
            public Light_types Type;
            public Vector4 Colour;
            public float Intensity;

            //Shader stuff below
            public Shader shaderL;
            public static int lightCount = 0;
            public int typeLoc;
            public int enabledLoc;
            public int positionLoc;
            public int targetLoc;
            public int colorLoc;
            public int intensityLoc;

            public Lights(Vector3 pos, Vector3 targ, bool enbl, Light_types typ, Raylib_cs.Color colr, float intes, Shader shd)
            {
                Position = pos;
                Target = targ;
                Enabled = enbl;
                Type = typ;
                
                Colour = new Vector4(colr.R / 255.0f, colr.G / 255.0f, colr.B / 255.0f, colr.A / 255.0f);
                /*              Colour[0] = (float)colr.R/255.0f;
                                Colour[1] = (float)colr.G/255.0f;
                                Colour[2] = (float)colr.B/255.0f;
                                Colour[3] = (float)colr.A/255.0f;*/
                Intensity = intes;
                shaderL = shd;

                enabledLoc = GetShaderLocation(shd, "lights[" + lightCount + "].enabled");
                typeLoc = GetShaderLocation(shd, "lights[" + lightCount + "].type");
                positionLoc = GetShaderLocation(shd, "lights[" + lightCount + "].position");
                targetLoc = GetShaderLocation(shd, "lights[" + lightCount + "].target");
                colorLoc = GetShaderLocation(shd, "lights[" + lightCount + "].color");
                intensityLoc = GetShaderLocation(shd, "lights[" + lightCount + "].intensity");


                System.Console.WriteLine($"||LIGHT PASS {lightCount}............ " + positionLoc + ".");
                lightCount++;
                
            }

            public static void UpdateShaderValues(Lights light_sc, Shader shade_sc)
            {
                SetShaderValue(shade_sc, light_sc.enabledLoc, light_sc.Enabled ? 1 : 0, ShaderUniformDataType.Int);

                SetShaderValue(shade_sc, light_sc.typeLoc, (int)light_sc.Type, ShaderUniformDataType.Int);

                SetShaderValue(shade_sc, light_sc.positionLoc, light_sc.Position, ShaderUniformDataType.Vec3);

                SetShaderValue(shade_sc, light_sc.targetLoc, light_sc.Target, ShaderUniformDataType.Vec3);

                SetShaderValue(shade_sc, light_sc.colorLoc, light_sc.Colour, ShaderUniformDataType.Vec4);

                SetShaderValue(shade_sc, light_sc.intensityLoc, light_sc.Intensity, ShaderUniformDataType.Float);
            }

           

            
        }

    }
}