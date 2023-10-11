

using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct BulletSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();

        if (!Input.GetKey(KeyCode.Space))
        {
            return;
        }
        
        var playerTransform = SystemAPI.GetSingleton<Player>().Transform;
        var playerForward = SystemAPI.GetSingleton<Player>().Forward;
        
        var bullet = state.EntityManager.Instantiate(config.BulletPrefab);
        
        state.EntityManager.SetComponentData(bullet, new LocalTransform
        {
            Position = playerTransform.Position + (playerForward * 0.2f),
            Rotation = playerTransform.Rotation,
            Scale = 0.1f
        });
        
        state.EntityManager.SetComponentData(bullet, new Velocity
        {
            Value = math.mul(playerTransform.Rotation, new float3(0,1,0))
            // Value = new float3(1,1,0)
        });
    }
}