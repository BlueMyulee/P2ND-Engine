using System.Numerics;
using Engine.Game.Objects;
using Engine.Logics.Sub.PlayerCon.FirstPerson;
using Raylib_cs;
using static Raylib_cs.Raylib;
namespace Engine.Resource ///NEXT STEP IS TO COMPLETELY OVERHAUL INTO ECS SYSTEM.
{
    public enum Shaders //ah fuck this existed? well shit, I should remove this next update
    {
        Mat_PBR_Metallic,
        Mat_Std_Specular,
        Mat_Fullbright
    }
    public class Materials //Uses Detected.
    {
        public Texture2D Albedo { get; set; }
        public Texture2D Normal { get; set; }
        public Texture2D Mrao { get; set; }

        public float albIntensity;
        public float rouIntensity;
        public float metIntensity;
        public float aoIntensity;
        public float emiIntensity;
        public Raylib_cs.Color emissioncolor;

        public Materials(Texture2D albd,Texture2D norm, Texture2D mr, float ai, float ri, float mi, float aoi, float ei)
        {
            Albedo = albd;
            Normal = norm;
            Mrao = mr;
            albIntensity = ai;
            rouIntensity = ri;
            metIntensity = mi;
            aoIntensity = aoi;
            emiIntensity = ei;
        }
    }
    public class Shadercl
    {
        public static Shader Mat_PBR;//Physically based rendering shader. bit overkill.
        public static Shader Mat_STD;//Used to serve a purpose.
        public static Shader Mat_FBR;//FULLBRIGHT
        public static Shader Mat_CEL;//CEL SHADING WOOHOOOOOOO!!! :DDD
        public static Shader Shadow;

        public static int emissiveIntensityLoc;
        public static int emissiveColorLoc;
        public static int textureTilingLoc;
        public static int lightVPLoc;
        public static int shadowMapLoc;
        public static int shadowmapResoultion = 1024;

        public static unsafe void InitializeShaderPBR()
        {
            //mostly OK
            var usage = 1;
            Mat_PBR = LoadShader("resources/shader/pbr.vs", "resources/shader/pbr.fs");
            Shadow = LoadShader("resources/shader/shadow.vs", null);
            Mat_PBR.Locs[(int)ShaderLocationIndex.MapAlbedo] = GetShaderLocation(Mat_PBR, "albedoMap");
            Mat_PBR.Locs[(int)ShaderLocationIndex.MapMetalness] = GetShaderLocation(Mat_PBR, "mraMap");
            Mat_PBR.Locs[(int)ShaderLocationIndex.MapNormal] = GetShaderLocation(Mat_PBR, "normalMap");
            Mat_PBR.Locs[(int)ShaderLocationIndex.MapEmission] = GetShaderLocation(Mat_PBR, "emissiveMap");
            Mat_PBR.Locs[(int)ShaderLocationIndex.ColorDiffuse] = GetShaderLocation(Mat_PBR, "albedoColor");

            //I think I can do with about like 7 light sources.
            Mat_PBR.Locs[(int)ShaderLocationIndex.VectorView] = GetShaderLocation(Mat_PBR, "viewPos");
            var lightCountLoc = GetShaderLocation(Mat_PBR, "numOfLights");
            var maxLightCount = 4;
            SetShaderValue(Mat_PBR, lightCountLoc, &maxLightCount, ShaderUniformDataType.Int); //You are the bane of my existance

            var ambientIntensity = 0.02f;
            var ambientColor = new Color(5, 4, 45, 225);
            var ambientColorNormalized = new Vector3(ambientColor.R / 255.0F, ambientColor.G / 255.0F, ambientColor.B / 255.0F);
            SetShaderValue(Mat_PBR, GetShaderLocation(Mat_PBR, "ambientColor"), &ambientColorNormalized, ShaderUniformDataType.Vec3);
            SetShaderValue(Mat_PBR, GetShaderLocation(Mat_PBR, "ambient"), &ambientIntensity, ShaderUniformDataType.Float);

            emissiveIntensityLoc = GetShaderLocation(Mat_PBR, "emissivePower");
            emissiveColorLoc = GetShaderLocation(Mat_PBR, "emissiveColor");
            textureTilingLoc = GetShaderLocation(Mat_PBR, "tiling"); //apparently I didn't set the default value as .5 by .5, See 98.
            lightVPLoc = GetShaderLocation(Mat_PBR, "lightVP");
            shadowMapLoc = GetShaderLocation(Mat_PBR, "shadowMap");
            Shadow.Locs[(int)ShaderLocationIndex.MatrixMvp] = GetShaderLocation(Shadow, "mvp");
                
            
            SetShaderValue(Mat_PBR, GetShaderLocation(Mat_PBR, "shadowMapResolution"), 1024, ShaderUniformDataType.Int);
            SetShaderValue(Mat_PBR, GetShaderLocation(Mat_PBR, "useTexAlbedo"), &usage, ShaderUniformDataType.Int);
            SetShaderValue(Mat_PBR, GetShaderLocation(Mat_PBR, "useTexNormal"), &usage, ShaderUniformDataType.Int);
            SetShaderValue(Mat_PBR, GetShaderLocation(Mat_PBR, "useTexMRA"), &usage, ShaderUniformDataType.Int);
            SetShaderValue(Mat_PBR, GetShaderLocation(Mat_PBR, "useTexEmissive"), &usage, ShaderUniformDataType.Int);
        }

        public static unsafe void ShaderUpdateRuntimePrePBR()
        {
            //I forgot what this does // I forgot too ngl
            var Shd_campos = ControlCorrespondant.camfps.Position; 
            SetShaderValue(Mat_PBR, Mat_PBR.Locs[(int)ShaderLocationIndex.VectorView], Shd_campos, ShaderUniformDataType.Vec3);
        }

        public static unsafe void ShaderUpdateRuntimeDuringPBR()
        {
            //THIS SHIT MAKES NO SENSE. I have to call tiling drawcalls for each model drawcalls right 1 frame before model drawcalls, absolutely ridiculous.
            var textile = new Vector2(0.5f, 0.5f);
            SetShaderValue(Mat_PBR, textureTilingLoc, &textile, ShaderUniformDataType.Vec2);
        }
         public static unsafe RenderTexture2D LoadShadowmapRenderTexture(int width, int height)
            {
                RenderTexture2D target = new();

                target.Id = Rlgl.LoadFramebuffer();
                target.Texture.Width = width;
                target.Texture.Height = height;

                if(target.Id > 0)
                {
                    
                    Rlgl.EnableFramebuffer(target.Id);
                    
                    target.Texture.Id = Rlgl.LoadTexture(null, width, height, PixelFormat.UncompressedR8G8B8A8, 1);
                    target.Texture.Width = width;
                    target.Texture.Height = height;
                    target.Texture.Format = PixelFormat.UncompressedR8G8B8A8;
                    target.Texture.Mipmaps = 1;
                    
                    bool useRenderBuffer = false;
                    target.Depth.Id = Rlgl.LoadTextureDepth(width, height, false);
                    target.Depth.Width = width;
                    target.Depth.Height = height;
                    target.Depth.Format = PixelFormat.UncompressedR32;
                    target.Depth.Mipmaps = 1;

                    Rlgl.FramebufferAttach(target.Id, target.Texture.Id, FramebufferAttachType.ColorChannel0, FramebufferAttachTextureType.Texture2D, 0);
                    Rlgl.FramebufferAttach(target.Id, target.Depth.Id, FramebufferAttachType.Depth, FramebufferAttachTextureType.Texture2D, 0);

                    if(Rlgl.FramebufferComplete(target.Id))
                    TraceLog(TraceLogLevel.Info, $"FBO: {target.Id} Framebuffer object created successfully");

                    Rlgl.DisableFramebuffer();
                }

                else TraceLog(TraceLogLevel.Warning, "Object Not created");

                return target;
            }

            public static void UnloadShadowmapRenderTexture(RenderTexture2D target)
            {
                {
                    if(target.Id > 0)
                    {
                        Rlgl.UnloadFramebuffer(target.Id);
                    }
                }
            }
    }
    
}
