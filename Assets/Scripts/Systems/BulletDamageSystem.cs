using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

[UpdateBefore(typeof(EnemyMoveSystem))]
public partial struct BulletDamageSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<Enemy>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        
        foreach(var (bulletTransform, bulletEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Bullet>().WithEntityAccess())
        {
            foreach (var (enemyTransform, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Enemy>().WithEntityAccess())
            {
                if(math.distancesq(bulletTransform.ValueRO.Position,enemyTransform.ValueRO.Position) > 0.1f)
                {
                    continue;
                }
                
                // hitting enemy
                
                if (config.DestroyBulletOnImpact) { 
                    SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(bulletEntity);
                }
                
                SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).DestroyEntity(enemyEntity);
            }
        }
        
    }




}