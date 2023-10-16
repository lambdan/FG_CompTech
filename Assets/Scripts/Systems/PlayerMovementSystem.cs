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
        var horizontal = Input.GetAxisRaw("Horizontal");
        var vertical = Input.GetAxisRaw("Vertical");

        if (horizontal == 0 && vertical == 0)
        {
            return; // no input == nothing to do
        }

        
        var config = SystemAPI.GetSingleton<Config>();
        var playerSingleton = SystemAPI.GetSingleton<Player>();
        var transform = SystemAPI.GetComponentRW<LocalTransform>(playerSingleton.Entity);
        
        // movement: https://stackoverflow.com/a/56622582

        // rotation
        var rot = transform.ValueRO.Rotation;
        if (horizontal != 0)
        {
            var newRot = math.mul(rot.value, quaternion.RotateZ(config.PlayerRotationSpeed * SystemAPI.Time.DeltaTime * horizontal * -1));
            transform.ValueRW.Rotation = newRot;
        }

        // movement
        var newForward = math.mul(rot.value, new float3(0, 1, 0));
        var newPos = transform.ValueRO.Position + vertical * newForward * SystemAPI.Time.DeltaTime * config.PlayerSpeed;

        // wrap around screen (like pac-man)
        if (math.abs(newPos.x) > 9)
        {
            newPos.x *= -1;
        }

        if (math.abs(newPos.y) > 5)
        {
            newPos.y *= -1;
        }

        transform.ValueRW.Position = newPos; // actually makes our character move
    }
}