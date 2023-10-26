using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class PlayerAuthoring : MonoBehaviour
{
    class Baker : Baker<PlayerAuthoring>
    {
        public override void Bake(PlayerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new Player
            {
                Entity = entity
            });
            
            AddComponent(entity, new PlayerHealth
            {
                Entity = entity
            });

        }
    }
}

public struct Player : IComponentData
{
    public Entity Entity;
}

public struct PlayerHealth : IComponentData
{
    public Entity Entity;
    public int Health;
    public double LastHitTime;
}