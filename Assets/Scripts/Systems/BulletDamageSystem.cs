using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;


public partial struct BulletDamageSystem : ISystem
{
    private EntityCommandBuffer ecb;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<Enemy>();
    }

    public void OnUpdate(ref SystemState state)
    {

        var config = SystemAPI.GetSingleton<Config>();
        ecb = new EntityCommandBuffer(Allocator.Temp);
        
        foreach(var (bulletTransform, bulletEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Bullet>().WithEntityAccess())
        {
            foreach (var (enemyTransform, enemyEntity) in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Enemy>().WithEntityAccess())
            {
                var bulletPos = bulletTransform.ValueRO.Position;
                var enemyPos = enemyTransform.ValueRO.Position;
                
                if (math.abs(bulletPos.x - enemyPos.x) > 0.1f || math.abs(bulletPos.y - enemyPos.y) > 0.1f)
                {
                    continue;
                }
                
                if (config.DestroyBulletOnImpact) { 
                    ecb.DestroyEntity(bulletEntity);
                }
                
                ecb.DestroyEntity(enemyEntity);

            }
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }




}