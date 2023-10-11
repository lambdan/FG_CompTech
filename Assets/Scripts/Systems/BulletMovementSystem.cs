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


[UpdateBefore(typeof(TransformSystemGroup))]
public partial struct BulletMovementSystem : ISystem
{
    private EntityCommandBuffer ecb;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<Config>();
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        ecb = new EntityCommandBuffer(Allocator.Temp);


        var dt = SystemAPI.Time.DeltaTime;
        
        foreach(var (bulletTransform, velocity, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRW<Velocity>>().WithAll<Bullet>().WithEntityAccess())
        {
            var newPos = bulletTransform.ValueRW.Position + (velocity.ValueRO.Value * dt * config.BulletSpeed);

            if (math.abs(newPos.x) > 10 || math.abs(newPos.y) > 10) // far away?
            {
                ecb.DestroyEntity(entity); // queue ourselves to be destroyed
                continue;
            }

            bulletTransform.ValueRW.Position = newPos;
        }
        
        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }




}