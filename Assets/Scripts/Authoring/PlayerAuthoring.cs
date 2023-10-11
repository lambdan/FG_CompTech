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

            AddComponent(entity, new Player{});
            
            AddComponent(entity, new PlayerHealth
            {
                Health = 100,
                LastDamage = 0
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
    public LocalTransform Transform;
    public float3 Forward;
}

public struct PlayerHealth : IComponentData
{
    public int Health;
    public double LastDamage;
}

public struct PlayerStats : IComponentData
{
    public int Kills;
    public int BulletsFired;
}