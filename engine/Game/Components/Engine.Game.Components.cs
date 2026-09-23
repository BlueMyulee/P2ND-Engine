using System;
namespace Engine.Game
{

    class Component
    {
        public Entity entity;

        public virtual void Update(float gameTime) {}
        public virtual void Setup() {}
    }
}