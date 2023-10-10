using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct SpawnSystem : ISystem
{
    uint updateCounter;
    private float lastSpawn;

    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        // This call makes the system not update unless at least one entity in the world exists that has the Spawner component.
        
        // TODO Change to look for player instead
        state.RequireForUpdate<SpawnerEntity>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var freq = SystemAPI.GetSingleton<SpawnerEntity>().frequency;
        
        if (SystemAPI.Time.ElapsedTime > (lastSpawn + freq))
        {
            var enemyEnt = SystemAPI.GetSingleton<SpawnerEntity>().enemyEntity;
            
            var amount = SystemAPI.GetSingleton<SpawnerEntity>().amount;
            
            var random = Random.CreateFromIndex(updateCounter++);

            for (int i=0; i < amount; i++)
            {
                Entity entity = state.EntityManager.Instantiate(enemyEnt);
                var transform = SystemAPI.GetComponentRW<LocalTransform>(entity);
                float2 position = (random.NextFloat2() - new float2(0.5f, 0.5f)) * 30;
                transform.ValueRW.Position = new float3(position.x, position.y, 0);
            }

            lastSpawn = (float)SystemAPI.Time.ElapsedTime;
        }
    }
}