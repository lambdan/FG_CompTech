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
    private EntityCommandBuffer _ecb;

    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Bullet>();
        state.RequireForUpdate<Config>();
        
    }

    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        _ecb = new EntityCommandBuffer(Allocator.Temp);
        
        foreach(var (bulletTransform, velocity, entity) in SystemAPI.Query<RefRW<LocalTransform>, RefRO<Velocity>>().WithAll<Bullet>().WithEntityAccess())
        {
            var newPos = bulletTransform.ValueRW.Position + (velocity.ValueRO.Value * SystemAPI.Time.DeltaTime * config.BulletSpeed);

            if (math.abs(newPos.x) > 10 || math.abs(newPos.y) > 10) // off screen?
            {
                _ecb.DestroyEntity(entity); // queue ourselves to be destroyed
                continue;
            }

            bulletTransform.ValueRW.Position = newPos;
        }
        
        _ecb.Playback(state.EntityManager);
        _ecb.Dispose();
    }




}