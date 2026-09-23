
using ComponentSystem;

namespace Engine.Game
{
    class ComponentBaseSystem<T> where T : Component
    {
        protected static List<T> components = new List<T>();

        public static void Register(T component)
        {
            components.Add(component);
            Console.WriteLine($"||INFO: ECS: Registered {typeof(T).Name}. Total count: {components.Count}");
        
        }

        public static void Update(float gameTime)
        {
            foreach(T component in components)
            {
                component.Update(gameTime);
            }
        }

        public static void Setup()
        {
            foreach(T component in components)
            {
                component.Setup();
            }
        }
    }

    class TransformSystem : ComponentBaseSystem<Transform>{ }
    class Mesh3DSystem : ComponentBaseSystem<ComponentSystem.Mesh3D>{ }
    class Mesh3DTransSystem : ComponentBaseSystem<ComponentSystem.Mesh3DTrans>{ }
    class LightDirSystem : ComponentBaseSystem<LightDir>{ }
    class SkyboxSystem : ComponentBaseSystem<Skybox> {}
}