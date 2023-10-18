using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;


public partial struct PlayerHealthSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<Enemy>();
        state.RequireForUpdate<PlayerHealth>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();

        var playerHealth = SystemAPI.GetSingletonRW<PlayerHealth>();

        if (playerHealth.ValueRO.LastHitTime + config.PlayerHitInvincibilitySeconds > SystemAPI.Time.ElapsedTime)
        {
            return; // hit invincibility in effect, nothing to do
        }

        var playerPos = SystemAPI.GetComponentRO<LocalTransform>(playerHealth.ValueRO.Entity).ValueRO.Position;

        foreach (var enemyTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Enemy>())
        {
            if(math.distancesq(playerPos,enemyTransform.ValueRO.Position) > 0.1f)
            {
                continue;
            }

            // enemy hit us
            playerHealth.ValueRW.Health -= 1;
            playerHealth.ValueRW.LastHitTime = SystemAPI.Time.ElapsedTime;

            if (playerHealth.ValueRO.Health <= 0)
            {
                Debug.Log("Player is dead! Game over!");
                // destroy the player
                SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(playerHealth.ValueRO.Entity);
            }

            return; // we took damage, dont need to check for any other enemies (since we're in invincibiliy time anyway)
        }
    }
    
}