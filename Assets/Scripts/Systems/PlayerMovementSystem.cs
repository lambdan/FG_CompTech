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

        // TODO surely there must be a way to get a single query
        foreach (var (transform,playerTag) in SystemAPI.Query<RefRW<LocalTransform>,RefRW<Player>>().WithAll<Player>())
        {
            
            // https://stackoverflow.com/a/56622582
            
            // rotation
            var rot = transform.ValueRO.Rotation;
            if (horizontal != 0)
            {
                var newRot = math.mul(rot.value, quaternion.RotateZ(config.PlayerRotationSpeed * SystemAPI.Time.DeltaTime * horizontal * -1));
                transform.ValueRW.Rotation = newRot;
            }

            // movement
            var newForward = math.mul(rot.value, new float3(0, 1, 0));
            playerTag.ValueRW.Forward = newForward;
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
            playerTag.ValueRW.Transform = transform.ValueRO; // enemies read from this (they can just use the singleton)
        }

    }
}