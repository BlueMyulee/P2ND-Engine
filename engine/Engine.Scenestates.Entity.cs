
using System.Numerics;
using Engine.Game.Resource.Load.Assets;
using Engine.Game.Resource.Load.IO;
namespace Engine.scenestate
{
    class Entity
    {
        public static void LoadMapClusterData(int mapClusterLocation)
        {
            var mapclust = Game.Resource.Load.IO.Assets.mapCl;

            for(int n = 0; n < mapclust[mapClusterLocation].entityRef.Length; n++)
            {
                SetupEntitiesInCluster(mapclust[mapClusterLocation].entityRef[n], new Vector3(mapclust[mapClusterLocation].entTransformX, mapclust[mapClusterLocation].entTransformY, mapclust[mapClusterLocation].entTransformZ));
            }
            
        }

        public static void SetupEntitiesInCluster(int addr, Vector3 transf)
        {
            var ents = Engine_Scenestates.Scenestate.entities;
            ents.Add(new Game.Entity());
            int curcount = ents.Count - 1;

            var entities = Assets.entFab[addr];

            for(int i = 0; i < entities.Components.Length; i++)
            {
                if(entities.Components[i].Contains(':'))
                {
                    
                }

            }
        }

    }
}