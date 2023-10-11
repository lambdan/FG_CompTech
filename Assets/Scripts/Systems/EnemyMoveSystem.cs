using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;
using Random = Unity.Mathematics.Random;

[BurstCompile]
public partial struct EnemyMoveSystem : ISystem
{
    private uint randomCounter;
    private float3 dir;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<Enemy>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        
        RefRO<LocalTransform> playerTransform = default;
        
        foreach (var transform in SystemAPI.Query<RefRO<LocalTransform>>().WithAll<Player>())
        {
            playerTransform = transform;
        }


        foreach (var transform in SystemAPI.Query<RefRW<LocalTransform>>().WithAll<Enemy>())
        {
            var playerPos = playerTransform.ValueRO.Position;
            dir = math.normalizesafe(playerPos - transform.ValueRO.Position);
            
            // jitter
            if (config.EnemyJitter)
            {
               dir += Random.CreateFromIndex(++randomCounter).NextFloat(-1f, 1f) * new float3(1, 1, 0); 
            }

            transform.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * config.EnemySpeed;

        }
    }




}