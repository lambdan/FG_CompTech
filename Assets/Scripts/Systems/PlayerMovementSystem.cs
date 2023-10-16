using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public partial struct PlayerMovementSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<Config>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
     
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            return; // no input == nothing to do
        }

        foreach (var (transform,p) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Player>>().WithAll<Player>())
        {
            
            // https://stackoverflow.com/a/56622582
            
            // rotation
            var rot = transform.ValueRO.Rotation;
            if (horizontal != 0)
            {
                var a = math.mul(rot.value, quaternion.RotateZ(config.PlayerRotationSpeed * SystemAPI.Time.DeltaTime * horizontal * -1));
                transform.ValueRW.Rotation = a;
            }

            // movement
            var forward = math.mul(rot.value, new float3(0, 1, 0));
            p.ValueRW.Forward = forward;
            var newPos = transform.ValueRO.Position + vertical * forward * SystemAPI.Time.DeltaTime * config.PlayerSpeed;

            if (math.abs(newPos.x) > 9)
            {
                newPos.x *= -1;
            }

            if (math.abs(newPos.y) > 5)
            {
                newPos.y *= -1;
            }
            
            transform.ValueRW.Position = newPos;

            p.ValueRW.Transform = transform.ValueRO;
        }

    }
}