using System.Numerics;
using Engine.Game;
using Engine.Game.Objects;
using Engine.Render.LightingEngine;
using Engine.Resource;
using Raylib_cs;

namespace ComponentSystem
{
    class LightDir : Component
    {
        GameObjects.Lights lights;
        public float intensity;
        public Vector3 target;
        public Raylib_cs.Color color;
        
        public LightDir()
        {
            LightDirSystem.Register(this);
        }

        public override void Setup()
        {
            lights = new GameObjects.Lights(entity.GetComponent<Transform>().position, target, true, Light_types.Directorional, this.color, intensity, Shadercl.Mat_PBR);
        }

         public override void Update(float gameTime)
        {
            Lighting.UpdateLight(Shadercl.Mat_PBR, this.lights);
        }
    }
}