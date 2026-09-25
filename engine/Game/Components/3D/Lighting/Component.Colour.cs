using System.Numerics;
using Engine.Game;
using Engine.Game.Objects;
using Engine.Render.LightingEngine;
using Engine.Resource;
using Raylib_cs;

namespace ComponentSystem
{
    class lightcolor : Component
    {
        public Raylib_cs.Color lcolor;
    }
}