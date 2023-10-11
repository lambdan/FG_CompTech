using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct BulletMoveSystem : ISystem
{
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<PlayerState>();
        state.RequireForUpdate<EnemyTag>();
    }
    
    public void OnUpdate(ref SystemState state)
    {
        foreach (var (transform, entity) in // get all entities with the EnemyTag
                 SystemAPI.Query<RefRW<LocalTransform>>()
                     .WithAll<BulletTag>()
                     .WithEntityAccess())

        {

            transform.ValueRW.Position += new float3(1, 1, 0);

        }
    }
}