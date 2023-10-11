using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct PlayerSpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        state.Enabled = false; // to only run this once

        var config = SystemAPI.GetSingleton<Config>();
        
        var player = state.EntityManager.Instantiate(config.PlayerPrefab);
        
        state.EntityManager.SetComponentData(player, new LocalTransform
        {
            Position = new float3(0,0,0),
            Rotation = quaternion.identity,
            Scale = 1
        });
    }
}