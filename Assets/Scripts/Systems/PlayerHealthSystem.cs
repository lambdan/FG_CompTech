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
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<PlayerHealth>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        
        if (SystemAPI.GetSingleton<PlayerHealth>().LastHitTime + config.PlayerHitInvincibilitySeconds > SystemAPI.Time.ElapsedTime)
        {
            return; // hit invincibility in effect, nothing to do
        }

         var playerPos = SystemAPI.GetComponentRO<LocalTransform>(SystemAPI.GetSingleton<Player>().Entity).ValueRO.Position;
        
        foreach (var enemyTransform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Enemy>())
        {
            var enemyPos = enemyTransform.ValueRO.Position;

            if (math.abs(playerPos.x - enemyPos.x) > 0.1f || math.abs(playerPos.y - enemyPos.y) > 0.1f)
            {
                continue;
            }
            
            // enemy hit us
            var playerHealth = SystemAPI.GetSingletonRW<PlayerHealth>(); // here we get RW singleton
            playerHealth.ValueRW.Health -= 1;
            playerHealth.ValueRW.LastHitTime = SystemAPI.Time.ElapsedTime;

            if (playerHealth.ValueRO.Health <= 0)
            {
                Debug.Log("Player is dead! Game over!");
                // TODO maybe trigger game over from here?

                // destroy the player
                SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(SystemAPI.GetSingleton<Player>().Entity);
            }
            
            return; // we took damage, dont need to check for any other enemies (since we're in invincibiliy time anyway)
        }
        
        
    }




}