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
            });
            
            AddComponent(entity, new PlayerStats
            {
                Kills = 0,
                BulletsFired = 0
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
    public int Health;
    public double LastHitTime;
}

public struct PlayerStats : IComponentData
{
    public int Kills;
    public int BulletsFired;
}