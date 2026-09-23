using System.Numerics;
using Engine.Game;
using Raylib_cs;
using static Raylib_cs.Raylib;
using Engine.Logics.Sub.PlayerCon.FirstPerson;

namespace ComponentSystem
{
    class Skybox : Component
    {
        public Skybox()
        {
            SkyboxSystem.Register(this);
        }

        public override void Update(float gameTime)
        {
            var trans = entity.GetComponent<Transform>();
            var loca = ControlCorrespondant.camfps.Position;
            trans.position = new Vector3(loca.X, 0, loca.Z); 
        }
    }
}