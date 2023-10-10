using System.Collections;
using System.Collections.Generic;
using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.VisualScripting;
using UnityEngine;

public partial struct EnemyMoveSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<EnemyData>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {


        foreach (var (transform, entity) in
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<EnemyMoveSpeed>()
                     .WithEntityAccess())
        {
            
            float3 dir = (SystemAPI.GetSingleton<PlayerPosition>().PlayerPos - transform.ValueRO.Position);
            dir = math.normalize(dir);
            transform.ValueRW.Position += dir * SystemAPI.Time.DeltaTime * SystemAPI.GetSingleton<EnemyData>().Speed;

        }
    }
}