using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public partial struct EnemySpawnerSystem : ISystem
{
    private double _lastSpawn;
    private const float TWO_PI = math.PI * 2.0f; // i cant remember what this is called lol
    
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<Config>();
        state.RequireForUpdate<Player>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var config = SystemAPI.GetSingleton<Config>();

        if (SystemAPI.Time.ElapsedTime > (_lastSpawn + config.EnemySpawnFrequency))
        {
            var angleSpacing = TWO_PI / config.EnemySpawnAmount;

            for (int i = 0; i < config.EnemySpawnAmount; i++)
            {
                Entity e = state.EntityManager.Instantiate(config.EnemyPrefab);

                // TODO maybe spawn using commandbuffer instead (cant set position here then though)
                // Entity e = SystemAPI.GetSingleton<BeginInitializationEntityCommandBufferSystem.Singleton>().CreateCommandBuffer(state.WorldUnmanaged).Instantiate(config.EnemyPrefab);

                float angle = angleSpacing * i;
                float x = config.EnemySpawnRadius * math.cos(angle);
                float y = config.EnemySpawnRadius * math.sin(angle);

                SystemAPI.GetComponentRW<LocalTransform>(e).ValueRW.Position = new float3(x, y, 0);
            }

            _lastSpawn = SystemAPI.Time.ElapsedTime;
        }
    }
}