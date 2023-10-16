using Unity.Burst;
using Unity.Entities;
using UnityEngine;


public partial struct InitializationSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<Player>();
        state.RequireForUpdate<PlayerHealth>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();
        SystemAPI.GetSingletonRW<PlayerHealth>().ValueRW.Health = config.PlayerHealth;

        Debug.Log("Initialization done");
        
        state.Enabled = false; // makes this only run once
    }




}