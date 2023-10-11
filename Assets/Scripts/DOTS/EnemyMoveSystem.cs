using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

[BurstCompile]
public partial struct EnemyMoveSystem : ISystem
{
    private float3 dir;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerState>();
        state.RequireForUpdate<EnemyTag>();
    }

    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, entity) in // get all entities with the EnemyTag
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<EnemyTag>()
                     .WithEntityAccess())

        {
            if (SystemAPI.GetSingleton<EnemyParams>().HuntPlayer && SystemAPI.GetSingleton<PlayerState>().PlayerAlive)
            {
                dir = math.normalizesafe(SystemAPI.GetSingleton<PlayerState>().PlayerPos - transform.ValueRO.Position);
            }
            
            transform.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * SystemAPI.GetSingleton<EnemyParams>().Speed;  

        }
    }




}